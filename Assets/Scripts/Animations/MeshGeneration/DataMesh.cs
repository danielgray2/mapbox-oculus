using Microsoft.Data.Analysis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class DataMesh : MonoBehaviour
{
    [SerializeField]
    Gradient meshGradient;

    [SerializeField]
    Material meshMaterial;

    public List<List<List<MeshDataObj>>> yList { get; private set; }
    public Transform parentViz { get; set; }
    GameObject dataMesh;

    // When passing in columns, the columns must be passed in
    // in descending order of how long they remain constant for.
    public void CreateMesh(DataObj dO, string yCol, string xCol, string zCol, string valCol)
    {
        // TODO: Change how we call this so that we only generate the mesh
        // if one isn't already generated
        List<string> colOrdering = FindColOrdering(dO, yCol, xCol, zCol);
        List<MeshDataObj> meshDataList = CreateDataList(dO, colOrdering[0], colOrdering[1], colOrdering[2], valCol);
        List<List<List<MeshDataObj>>> meshStruct = CreateMeshStructure(meshDataList);
        for (int i = 0; i < meshStruct.Count; i++)
        {
            GameObject go = new GameObject();
            go.transform.parent = this.transform;
            go.transform.position = Vector3.zero;
            go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>();
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            go.AddComponent<BoxCollider>();
            MeshLayer mL = go.AddComponent<MeshLayer>();
            mL.GenerateMesh(meshStruct[i], meshGradient, meshMaterial);
        }

        // Quick test I spun up
        /*
        int counter = 0;
        for (int i = 0; i < meshStruct.Count; i++)
        {
            for(int j = 0; j < meshStruct[i].Count; j++)
            {
                for (int k = 0; k < meshStruct[i][j].Count; k++)
                {
                    if (i + 1 < meshStruct.Count)
                    {
                        Assert.IsTrue(meshStruct[i][j][k].position.y < meshStruct[i + 1][0][0].position.y);
                    }
                    if (j + 1 < meshStruct[i].Count)
                    {
                        Assert.IsTrue(meshStruct[i][j][k].position.x < meshStruct[i][j+1][0].position.x);
                    }
                    if (k + 1 < meshStruct[i][j].Count)
                    {
                        Assert.IsTrue(meshStruct[i][j][k].position.z < meshStruct[i][j][k + 1].position.z);
                    }
                    counter++;
                }
            }
        }
        Debug.Log("Well, that is pretty neat: " + counter);
        */
    }

    public List<String> FindColOrdering(DataObj dO, string nameOne, string nameTwo, string nameThree)
    {
        List<String> colOrdering = new List<string>();

        int numNameOne = dO.CountNumVals(nameOne, float.Parse(dO.df.Columns[nameOne][0].ToString()));
        int numNameTwo = dO.CountNumVals(nameTwo, float.Parse(dO.df.Columns[nameTwo][0].ToString()));
        int numNameThree = dO.CountNumVals(nameThree, float.Parse(dO.df.Columns[nameThree][0].ToString()));

        SortedList<int, string> tempList = new SortedList<int, string>();
        tempList.Add(numNameOne, nameOne);
        tempList.Add(numNameTwo, nameTwo);
        tempList.Add(numNameThree, nameThree);

        colOrdering.Add(tempList.Values[0]);
        colOrdering.Add(tempList.Values[1]);
        colOrdering.Add(tempList.Values[2]);
        colOrdering.Reverse();

        return colOrdering;
    }

    protected List<MeshDataObj> CreateDataList(DataObj dO, string yCol, string xCol, string zCol, string valCol)
    {
        List<MeshDataObj> dataList = new List<MeshDataObj>();
        DataFrame df = dO.df;
        for(int i = 0; i < df.Rows.Count; i++)
        {
            float xPos = float.Parse(df.Columns[xCol][i].ToString());
            float yPos = float.Parse(df.Columns[yCol][i].ToString());
            float zPos = float.Parse(df.Columns[zCol][i].ToString());
            float rawVal = float.Parse(df.Columns[valCol][i].ToString());
            float val = Mathf.InverseLerp(dO.GetMin(valCol), dO.GetMax(valCol), rawVal);
            Vector3 currPos = new Vector3(xPos, yPos, zPos);
            MeshDataObj mDO = new MeshDataObj(val, currPos);
            dataList.Add(mDO);
        }
        return dataList;
    }

    // This can be optimized by sorting each depth, then each
    // lon grouped by depth, then each lat grouped by lon
    public List<List<List<MeshDataObj>>> CreateMeshStructure(List<MeshDataObj> meshDataList)
    {
        string strForm = "0.000";
        Dictionary<string, Dictionary<string, Dictionary<string, List<MeshDataObj>>>> dataDict = new Dictionary<string, Dictionary<string, Dictionary<string, List<MeshDataObj>>>>();

        // Get the depths
        HashSet<float> yHashSet = new HashSet<float>();
        foreach (MeshDataObj mD in meshDataList)
        {
            yHashSet.Add(mD.position.y);
        }

        foreach (float yVal in yHashSet)
        {
            Dictionary<string, Dictionary<string, List<MeshDataObj>>> subDict = new Dictionary<string, Dictionary<string, List<MeshDataObj>>>();
            dataDict.Add(yVal.ToString(strForm), subDict);
        }

        // Add the longitudes
        foreach (MeshDataObj mD in meshDataList)
        {
            Dictionary<string, List<MeshDataObj>> subDict = new Dictionary<string, List<MeshDataObj>>();
            if (!dataDict[mD.position.y.ToString(strForm)].ContainsKey(mD.position.x.ToString(strForm)))
                dataDict[mD.position.y.ToString(strForm)].Add(mD.position.x.ToString(strForm), subDict);
        }

        // Add the latitudes
        foreach (MeshDataObj mD in meshDataList)
        {
            List<MeshDataObj> subList = new List<MeshDataObj>();
            if (!dataDict[mD.position.y.ToString(strForm)][mD.position.x.ToString(strForm)].ContainsKey(mD.position.z.ToString(strForm)))
                dataDict[mD.position.y.ToString(strForm)][mD.position.x.ToString(strForm)].Add(mD.position.z.ToString(strForm), subList);
        }

        // Add the data MeshDataObjs
        foreach (MeshDataObj mD in meshDataList)
        {
            dataDict[mD.position.y.ToString(strForm)][mD.position.x.ToString(strForm)][mD.position.z.ToString(strForm)].Add(mD);
        }

        // Convert dataDict to 3D List
        List<List<List<MeshDataObj>>> yList = new List<List<List<MeshDataObj>>>();
        List<string> yKeys = SortStringListAsFloats(dataDict.Keys.ToList(), strForm);
        foreach (string yKey in yKeys)
        {
            List<List<MeshDataObj>> xList = new List<List<MeshDataObj>>();
            List<string> xKeys = SortStringListAsFloats(dataDict[yKey].Keys.ToList(), strForm);
            foreach (string xKey in xKeys)
            {
                List<MeshDataObj> zList = new List<MeshDataObj>();
                List<string> zKeys = SortStringListAsFloats(dataDict[yKey][xKey].Keys.ToList(), strForm);
                foreach (string zKey in zKeys)
                {
                    foreach (MeshDataObj mD in dataDict[yKey][xKey][zKey])
                    {
                        zList.Add(mD);
                    }
                }
                xList.Add(zList);
            }
            yList.Add(xList);
        }
        this.yList = yList;
        return yList;
    }

    public List<string> SortStringListAsFloats(List<string> stringList, string strFormat)
    {
        List<string> returnList = new List<string>();
        List<float> floatList = new List<float>();
        foreach (string currString in stringList)
        {
            float asFloat = float.Parse(currString, CultureInfo.InvariantCulture.NumberFormat);
            floatList.Add(asFloat);
        }
        floatList.Sort();
        foreach (float currFloat in floatList)
        {
            returnList.Add(currFloat.ToString(strFormat));
        }
        return returnList;
    }

    private void Update()
    {
        if(parentViz != null)
        {
            dataMesh.transform.position = parentViz.position;
            dataMesh.transform.localScale = parentViz.localScale;
        }
    }
}