using UnityEngine;

public class KamikazeAI : MonoBehaviour {
    //**        Attach to Enemy     **//

    private float detectionTimer;
    public float autoTimer;
    private PatrolAi patrol;
    private EnemyBehavior behavior;
    // Use this for initialization
    void Start()
    {
        detectionTimer = 0f;
        autoTimer = 0f;
        patrol = GetComponent<PatrolAi>();
        behavior = GetComponent<EnemyBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (detectionTimer > 0f)
            detectionTimer -= Time.deltaTime;

        if (autoTimer > 0f)
            autoTimer -= Time.deltaTime;
        else
        {
            if (autoTimer < 0f)
                patrol.AutoPilot = false;

            autoTimer = 0f;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.name != transform.name && patrol.SecondaryTarget == null)
        {
            if (col.CompareTag("Enemy") && col.GetType() == typeof(SphereCollider))
            {                
                Debug.Log("Kamikaze patrolling with : " + col.name);
                patrol.SecondaryTarget = col.transform;
            }
        }     
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Enemy") && col.GetType() == typeof(SphereCollider))
        {
            if (behavior.Target == null) {
                Debug.Log("Heading Back to Enemy");
                patrol.AutoPilot = true;
                autoTimer = 5f;
            }
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.CompareTag("Player") && detectionTimer <= 0f)
        {
            detectionTimer = 1f;
            Debug.Log("Droid Blew Up");
            hit.transform.SendMessage("Hit");
            hit.transform.SendMessage("EMPHit");
            transform.SendMessage("Kill");
        }
    }
}