using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class GenerateScatterplot : MonoBehaviour
{
    [SerializeField]
    GameObject scatterplotPrefab;

    SData dataOne = new SData();
    SData dataTwo = new SData();
    SData dataThree = new SData();
    SData dataFour = new SData();
    SData dataFive = new SData();
    SData dataSix = new SData();

    List<SData> dataList = new List<SData>();

    private void Start()
    {
        SetupData();
        Vector3 plotPosition = Camera.main.transform.right * 2;
        plotPosition.y = 1;
        GameObject scatterplotGo = Instantiate(scatterplotPrefab, plotPosition, Quaternion.identity);
        //scatterplotGo.transform.parent = cube.transform;
        Renderer plotRenderer = scatterplotGo.GetComponent<Renderer>();
        //cubeRenderer.bounds = plotRenderer.bounds.extents;
        ScatterBox sPObj = scatterplotGo.GetComponentInChildren<ScatterBox>();
        sPObj.InitializeScatterplot(dataList, "lat", "lon", "ccmadRatio");
    }

    private void SetupData()
    {
        dataOne.ccmadRatio = 0.5f;
        dataOne.lat = 45f;
        dataOne.lon = 110f;

        dataTwo.ccmadRatio = 0.4f;
        dataTwo.lat = 50f;
        dataTwo.lon = 100f;

        dataThree.ccmadRatio = 0.25f;
        dataThree.lat = 48f;
        dataThree.lon = 105f;

        dataFour.ccmadRatio = 0.8f;
        dataFour.lat = 60f;
        dataFour.lon = 111f;

        dataFive.ccmadRatio = 0.2f;
        dataFive.lat = 51f;
        dataFive.lon = 106f;

        dataSix.ccmadRatio = 0.3f;
        dataSix.lat = 48.73f;
        dataSix.lon = 104f;

        dataList.Add(dataOne);
        dataList.Add(dataTwo);
        dataList.Add(dataThree);
        dataList.Add(dataFour);
        dataList.Add(dataFive);
        dataList.Add(dataSix);
    }
}
