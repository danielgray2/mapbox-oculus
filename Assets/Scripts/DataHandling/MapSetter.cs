using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Rework map so that it follows the pattern
// used by scatterplot
public class MapSetter : MonoBehaviour
{
    [SerializeField]
    GameObject map;

    [SerializeField]
    Gradient meshColorGradient;

    [SerializeField]
    Material meshMaterial;
    Vector3 originalMapScale = new Vector3(0.17f, 0.17f, 0.17f);
    Vector3 originalMapPosition = new Vector3(-2f, 1f, 0f);

    protected GameObject layerParent;
    void Start()
    {
        MapStore.Instance.map = map.GetComponent<AbstractMap>();
        map.transform.localScale = originalMapScale;
        map.transform.position = originalMapPosition;
        CreateTheMesh();
    }

    private void CreateTheMesh()
    {
        // TODO: Change how we call this so that we only generate the mesh
        // if one isn't already generated
        List<List<List<VData>>> meshStruct = DataStore.Instance.CreateMeshStructure();
        layerParent = new GameObject();
        layerParent.transform.position = Vector3.zero;
        layerParent.name = "MeshLayers";
        for(int i = 0; i < meshStruct.Count; i++)
        {
            GameObject go = new GameObject();
            go.transform.position = Vector3.zero;
            //go.transform.parent = layerParent.transform;
            go.transform.parent = MapStore.Instance.map.transform;
            go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>();
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            go.AddComponent<BoxCollider>();
            MeshGenerator mg = go.AddComponent<MeshGenerator>();
            mg.GenerateMesh(meshStruct[i], meshColorGradient, meshMaterial);
        }
        
        // Quick test I spun up
        /*
        int counter = 0;
        for (int i = 0; i < meshStruct.Count; i++)
        {
            Debug.Log("Inside first");
            for(int j = 0; j < meshStruct[i].Count; j++)
            {
                Debug.Log("Inside second");
                for (int k = 0; k < meshStruct[i][j].Count; k++)
                {
                    Debug.Log("Inside third");
                    if (i + 1 < meshStruct.Count)
                    {
                        Assert.IsTrue(meshStruct[i][j][k].depth < meshStruct[i + 1][0][0].depth);
                    }
                    if (j + 1 < meshStruct[i].Count)
                    {
                        Assert.IsTrue(meshStruct[i][j][k].lon < meshStruct[i][j+1][0].lon);
                    }
                    if (k + 1 < meshStruct[i][j].Count)
                    {
                        Assert.IsTrue(meshStruct[i][j][k].lat < meshStruct[i][j][k + 1].lat);
                    }
                    counter++;
                }
            }
        }
        Debug.Log("Well, that is pretty neat: " + counter);
        */
    }

    /*
    private void Update()
    {
        layerParent.transform.position = MapStore.Instance.map.transform.position;
        layerParent.transform.localScale = MapStore.Instance.map.transform.localScale;
    }
    */
}
