using Mapbox.Map;
using Mapbox.Unity.Map;
using Mapbox.Unity.Map.Strategies;
using Mapbox.Utils;
using Microsoft.Data.Analysis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiguringOutMapBox : MonoBehaviour
{
    [SerializeField]
    public Texture2D loadingTexture;

    [SerializeField]
    public Material tileMaterial;
    
    // Start is called before the first frame update
    void Start()
    {
    }
}
