using UnityEngine;

[RequireComponent(typeof(EnemyBehavior))]
public class TransportShipAI : MonoBehaviour
{
    #region Properties
    //  Transport Data
    private float padding;
    public float cloakTimer;

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

        mesh1 = transform.GetChild(0).GetChild(0).GetComponent<Renderer>();
        mesh2 = transform.GetChild(0).GetChild(1).GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update () {
        if (padding > 0f)
            padding -= Time.deltaTime;

        if (cloakTimer > 0.0f)
            cloakTimer -= Time.deltaTime;
        else {
            if(cloakTimer < 0.0f)
                CloakOff();

            cloakTimer = 0.0f;
        }
    }

    void CloakOn() {
        if (cloakTimer <= 0.0f) {
            cloakTimer = 30.0f;
            mesh1.material = transMat;
            mesh2.material = transMat;
        }
    }

    void CloakOff() {
        behavior.ChangeBehavior();
        mesh1.material = opaqueMat;
        mesh2.material = opaqueMat;
    }

    void OnTriggerEnter(Collider col) {
        if (col.tag == "Player")
            CloakOn();
    }

    public float GetCloakTimer()
    {
        return cloakTimer;
    }
}
