using UnityEngine;

public class TransportAi : MonoBehaviour
{
    #region Properties
    //  Transport Data
    public Material transMat;
    public Material opaqueMat;

    private Renderer mesh1;
    private Renderer mesh2;

    // Enemy Data
    private EnemyBehavior behavior;
    #endregion


    // Use this for initialization
    void Start () {
        behavior = GetComponent<EnemyBehavior>();
        behavior.SetUniqueAi(this);

        mesh1 = transform.GetChild(3).GetChild(0).GetComponent<Renderer>();
        mesh2 = transform.GetChild(3).GetChild(1).GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update () {

    }

    void CloakOn()
    {
        mesh1.material = transMat;
        mesh2.material = transMat;
        Invoke("CloakOff", 30f);
    }

    void CloakOff() {
        behavior.ChangeBehavior();
        mesh1.material = opaqueMat;
        mesh2.material = opaqueMat;
    }

    void OnTriggerEnter(Collider col) {
        if (col.CompareTag("Player"))
            CloakOn();
    }
}
