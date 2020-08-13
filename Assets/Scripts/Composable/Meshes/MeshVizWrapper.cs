using System;
using System.Collections.Generic;
using UnityEngine;

public class MeshVizWrapper : IAbstractWrapper
{
    [SerializeField]
    Material meshMaterial;

    [SerializeField]
    Gradient gradient;

    protected MeshViz wrapped;
    protected bool initialized = false;
    protected bool firstRun = true;

    // Update is called once per frame
    void Update()
    {
        if (initialized && firstRun)
        {
            MeshModel meshModel = CastToMeshVizModel();
            DrawMesh(wrapped.CreateMesh(meshModel));
            firstRun = false;
        }
    }

    public void Initialize(MeshModel meshModel)
    {
        model = meshModel;
        wrapped = new MeshViz();
        this.gradient = gradient;
        initialized = true;
    }

    public void DrawMesh(List<List<List<MeshDataObj>>> meshStruct)
    {
        List<MeshVizLayer> meshLayerList = new List<MeshVizLayer>();
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
            MeshVizLayer mL = go.AddComponent<MeshVizLayer>();
            mL.GenerateMesh(meshStruct[i], gradient, meshMaterial);
            meshLayerList.Add(mL);
        }
        MeshModel meshModel = CastToMeshVizModel();
        meshModel.meshLayerList = meshLayerList;
    }

    MeshModel CastToMeshVizModel()
    {
        if(!(this.model is MeshModel meshModel))
        {
            throw new ArgumentException("Model must be of type MeshVizModel");
        }
        return meshModel;
    }

    public override void Create()
    {
        Debug.Log("Well that happened");
    }
}
