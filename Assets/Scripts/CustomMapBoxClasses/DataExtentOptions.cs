using Mapbox.Unity.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataExtentOptions : ExtentOptions
{
	public DataObj dataObj;
	public string latColName;
	public string lonColName;
	public override void SetOptions(ExtentOptions extentOptions)
	{
		DataExtentOptions options = extentOptions as DataExtentOptions;
		if (options != null)
		{
			dataObj = options.dataObj;
			latColName = options.latColName;
			lonColName = options.lonColName;
		}
		else
		{
			Debug.LogError("ExtentOptions type mismatch : Using " + extentOptions.GetType() + " to set extent of type " + this.GetType());
		}
	}

	public void SetOptions(DataObj dataObj, string latColName, string lonColName)
	{
		this.dataObj = dataObj;
		this.latColName = latColName;
		this.lonColName = lonColName;
	}
}
