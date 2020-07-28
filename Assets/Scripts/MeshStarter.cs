using Microsoft.Data.Analysis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshStarter : MonoBehaviour
{
    [SerializeField]
    GameObject dataMeshPrefab;
    // Start is called before the first frame update
    void Start()
    {
        GameObject gO = Instantiate(dataMeshPrefab);
        gO.transform.position = Vector3.zero;
        gO.transform.localScale = Vector3.one;
        DataMesh dM = gO.GetComponent<DataMesh>();

        DataFrame df = DataFrame.LoadCsv("Assets\\Resources\\coso_velo.csv");
        DataObj dO = new DataObj(df);

        dM.CreateMesh(dO, "depth(km)", "lon", "lat", "Vp/Vs");
    }
}
