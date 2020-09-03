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
    protected bool plottedOnce = false;
    protected GameObject go;

    // Update is called once per frame
    public void Plot()
    {
        if (initialized && !plottedOnce)
        {
            MeshModel meshModel = CastToMeshVizModel();
            DrawMesh(wrapped.CreateMesh(meshModel));
            plottedOnce = true;
        }
    }

    public override void ReRender()
    {
        Destroy(go);
        MeshModel meshModel = CastToMeshVizModel();
        DrawMesh(wrapped.CreateMesh(meshModel));
    }

    public void Initialize(MeshModel meshModel)
    {
        model = meshModel;
        model.modelUpdateEvent.AddListener(HandleModelUpdate);
        wrapped = new MeshViz();
        initialized = true;
    }

    public void DrawMesh(List<List<List<MeshDataObj>>> meshStruct)
    {
        List<MeshVizLayer> meshLayerList = new List<MeshVizLayer>();
        for (int i = 0; i < meshStruct.Count; i++)
        {
            go = new GameObject();
            go.transform.parent = this.transform;
            go.transform.localPosition = Vector3.zero;
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
