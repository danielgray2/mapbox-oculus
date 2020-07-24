using CsvHelper.Configuration;
using Mapbox.Unity.Utilities;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    List<Vector3> vertices;
    List<VData> dataPoints;
    List<int> triangles;
    Gradient gradient;

    List<Color> colors;

    public void GenerateMesh(List<List<VData>> dataMesh, Gradient gradient, Material meshMaterial)
    {
        /*
        this.gradient = new Gradient();
        GradientColorKey[] colorKey;
        GradientAlphaKey[] alphaKey;

        colorKey = new GradientColorKey[2];
        colorKey[0].color = Color.red;
        colorKey[0].time = 0.0f;
        colorKey[1].color = Color.blue;
        colorKey[1].time = 1.0f;

        alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 0.0f;
        alphaKey[1].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);
        */
        this.gradient = gradient;

        mesh = new Mesh();
        List<string> stringList = new List<string>();
        for(int i = 0; i < dataMesh.Count; i++)
        {
            string currString = "";
            for(int j = 0; j < dataMesh[i].Count; j++)
            {
                currString += dataMesh[i][j].GetUnityPosition();
                currString += "||";
            }
            stringList.Add(currString);
        }
        System.IO.File.WriteAllLines(@"C:\Users\grayd\Documents\WriteLines.txt", stringList);
        GetComponent<MeshFilter>().mesh = mesh;
        Renderer renderer = GetComponent<Renderer>();
        renderer.material = meshMaterial;
        renderer.receiveShadows = false;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        CreateShape(dataMesh);
        UpdateMesh();
    }

    void CreateShape(List<List<VData>> dataMesh)
    {
        // Vertices holds the positions and dataPoints
        // holds the actual data. Their indices must
        // match each other.
        vertices = new List<Vector3>();
        dataPoints = new List<VData>();
        
        // DataMesh is our array of longitudes
        for(int k = 0; k < dataMesh.Count; k++)
        {
            for(int j = 0; j < dataMesh[k].Count; j++)
            {
                VData dataPoint = dataMesh[k][j];
                vertices.Add(dataPoint.GetUnityPosition());
                dataPoints.Add(dataPoint);
            }
        }

        triangles = new List<int>();

        int vert = 0;
        for(int i = 0; i < dataMesh.Count-1; i++)
        {
            for (int j = 0; j < dataMesh[i].Count-1; j++)
            {
                triangles.Add(vert + 0);
                triangles.Add(vert + dataMesh[i].Count);
                triangles.Add(vert + 1);
                triangles.Add(vert + 1);
                triangles.Add(vert + dataMesh[i].Count);
                triangles.Add(vert + dataMesh[i].Count + 1);

                vert++;
            }
            vert++;
        }

        triangles.Reverse();

        colors = new List<Color>();
        //System.Random rand = new System.Random();
        for (int i = 0, k = 0; k < dataMesh.Count; k++)
        {
            for (int j = 0; j < dataMesh[k].Count; j++)
            {
                float value = Mathf.InverseLerp(DataStore.Instance.minVpVs, DataStore.Instance.maxVpVs, dataPoints[i].vPvS);
                //float value = (float)rand.NextDouble();
                colors.Add(gradient.Evaluate(value));
            }
            // Hmm... looking back at this, it looks like i should be in the inner for loop
            // Look into this after you refactor
            i++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.colors = colors.ToArray();

        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
            return;
        for(int i = 0; i < vertices.Count; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}
