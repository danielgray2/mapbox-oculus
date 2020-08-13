﻿using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshVizLayer : IAbstractView
{
    Mesh mesh;
    List<Vector3> vertices;
    List<MeshDataObj> dataPoints;
    List<int> triangles;
    Gradient gradient;

    List<Color> colors;

    public void GenerateMesh(List<List<MeshDataObj>> dataMesh, Gradient gradient, Material meshMaterial)
    {
        this.gradient = gradient;

        mesh = new Mesh();
        /*
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
        */
        GetComponent<MeshFilter>().mesh = mesh;
        Renderer renderer = GetComponent<Renderer>();
        renderer.material = meshMaterial;
        renderer.receiveShadows = false;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        CreateShape(dataMesh);
        UpdateMesh();
    }

    void CreateShape(List<List<MeshDataObj>> dataMesh)
    {
        // Vertices holds the positions and dataPoints
        // holds the actual data. Their indices must
        // match each other.
        vertices = new List<Vector3>();
        dataPoints = new List<MeshDataObj>();

        // DataMesh is our array of longitudes
        for (int k = 0; k < dataMesh.Count; k++)
        {
            for (int j = 0; j < dataMesh[k].Count; j++)
            {
                MeshDataObj dataPoint = dataMesh[k][j];
                vertices.Add(dataPoint.position);
                dataPoints.Add(dataPoint);
            }
        }

        triangles = new List<int>();

        int vert = 0;
        for (int i = 0; i < dataMesh.Count - 1; i++)
        {
            for (int j = 0; j < dataMesh[i].Count - 1; j++)
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
        for (int i = 0, k = 0; k < dataMesh.Count; k++)
        {
            for (int j = 0; j < dataMesh[k].Count; j++)
            {
                float value = dataPoints[i].colorValue;
                colors.Add(gradient.Evaluate(value));
                i++;
            }
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
        for (int i = 0; i < vertices.Count; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }

    public override void Initialize(IAbsModel iAbsModel) {}
}
