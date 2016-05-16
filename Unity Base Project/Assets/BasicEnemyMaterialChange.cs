using UnityEngine;
using System.Collections;

public class BasicEnemyMaterialChange : MonoBehaviour {


    [SerializeField]
    private Material OutlineMaterial;
    [SerializeField]
    private Material NormalMaterial;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}



    void ChangeMateralToOutlined()
    {
        gameObject.GetComponent<MeshRenderer>().material = OutlineMaterial;
    }

    void ChangeMateralToNormal()
    {
        gameObject.GetComponent<MeshRenderer>().material = NormalMaterial;
    }
}
