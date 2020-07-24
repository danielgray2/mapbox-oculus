using System.Collections.Generic;
using Mapbox.Map;
using Mapbox.Unity.Map.TileProviders;
using Mapbox.Utils;
using UnityEngine;

public class DataTileProvider : AbstractTileProvider
{
    private DataExtentOptions _dataExtentOptions;
    private bool _initialized = false;

    public override void OnInitialized()
    {
        if (Options != null)
        {
            _dataExtentOptions = (DataExtentOptions)Options;
        }
        else
        {
            _dataExtentOptions = new DataExtentOptions();
        }

        _initialized = true;
        _currentExtent.activeTiles = new HashSet<UnwrappedTileId>();
    }

    public override void UpdateTileExtent()
    {
        if (!_initialized || _dataExtentOptions == null)
        {
            return;
        }

        _currentExtent.activeTiles.Clear();
        // Latitude north and south
        // Longitude east and west

        float westBound = _dataExtentOptions.dataObj.GetMin(_dataExtentOptions.lonColName);
        float eastBound = _dataExtentOptions.dataObj.GetMax(_dataExtentOptions.lonColName);
        float southBound = _dataExtentOptions.dataObj.GetMin(_dataExtentOptions.latColName);
        float northBound = _dataExtentOptions.dataObj.GetMax(_dataExtentOptions.latColName);

        Vector2d southWest = new Vector2d(southBound, westBound);
        Vector2d northEast = new Vector2d(northBound, eastBound);

        Vector2dBounds bounds = new Vector2dBounds(southWest, northEast);
        var tileCover = TileCover.Get(bounds, _map.AbsoluteZoom);

        foreach(CanonicalTileId tileId in tileCover)
        {
            var currTile = new UnwrappedTileId(_map.AbsoluteZoom, tileId.X, tileId.Y);
            _currentExtent.activeTiles.Add(currTile);
        }

        OnExtentChanged();
    }

    public override bool Cleanup(UnwrappedTileId tile)
    {
        return (!_currentExtent.activeTiles.Contains(tile));
    }
}
