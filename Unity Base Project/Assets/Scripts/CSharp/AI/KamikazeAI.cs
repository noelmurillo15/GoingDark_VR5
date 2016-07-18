using UnityEngine;

public class KamikazeAI : MonoBehaviour
{
    #region Properties
    //  Kami Data
    private int typebot;
    private float detectionTimer;
    private float selfdestructTimer;

    //  Enemy Data
    private PatrolAi patrol;
    private EnemyStateManager behavior;
    #endregion


    // Use this for initialization
    void Start()
    {
        behavior = GetComponent<EnemyStateManager>();
        patrol = GetComponent<PatrolAi>();
        behavior.SetUniqueAi(this);
        selfdestructTimer = 20f;
        detectionTimer = 0f;

        typebot = Random.Range(0, 1);  
    }

    // Update is called once per frame
    void Update()
    {
        if (detectionTimer > 0f)
            detectionTimer -= Time.deltaTime;
    }

    #region Self-Destruct
    void SelfDestruct()
    {
        Invoke("Explosion", selfdestructTimer);
    }
    void SelfDestructBoss()
    {
        Invoke("Explosion", 20);
    }
    void Explosion()
    {
        GameObject go = Instantiate(Resources.Load("Particles/Boom"), behavior.MyTransform.position, Quaternion.identity) as GameObject;
        go.transform.parent = behavior.MyTransform.parent;
        SendMessage("Kill");
    }
    #endregion

    #region Collision Detection
    void OnCollisionEnter(Collision hit)
    {
        if (hit.transform.CompareTag("Player") && detectionTimer <= 0f)
        {
            detectionTimer = 1f;
            if (typebot == 0)
                hit.transform.SendMessage("EMPHit");
            else
                hit.transform.SendMessage("ShieldHit");
            Explosion();
        }
    }
    #endregion
}