using UnityEngine;
using GD.Core.Enums;

public class KamikazeAI : MonoBehaviour
{
    #region Properties
    //  Kami Data
    public float autoTimer;
    private float detectionTimer;
    private float selfdestructTimer;

    //  Enemy Data
    private bool empBot;
    private bool explodeBot;
    private PatrolAi patrol;
    private EnemyBehavior behavior;
    #endregion


    // Use this for initialization
    void Start()
    {
        behavior = GetComponent<EnemyBehavior>();
        patrol = GetComponent<PatrolAi>();
        behavior.SetUniqueAi(this);
        selfdestructTimer = 20f;
        detectionTimer = 0f;
        autoTimer = 0f;

        if (GetComponent<Light>().color == Color.red)
            explodeBot = true;
        else
            empBot = true;        
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
            //if (autoTimer < 0f)
            //    patrol.AutoPilot = false;

            autoTimer = 0f;
        }
    }

    #region Self-Destruct
    void SelfDestruct()
    {
        Invoke("Explosion", selfdestructTimer);
    }

    private void Explosion()
    {
        GameObject go = Instantiate(Resources.Load("Particles/Boom"), behavior.MyTransform.position, Quaternion.identity) as GameObject;
        go.transform.parent = behavior.MyTransform.parent;
        DestroyObject(gameObject);
    }
    #endregion

    #region Collision Detection
    //void OnTriggerEnter(Collider col)
    //{
    //    if (col.transform.name != transform.name/* && patrol.SecondaryTarget == null*/)
    //    {
    //        if (col.CompareTag("Enemy") && col.GetType() == typeof(SphereCollider))
    //        {                
    //            Debug.Log("Kamikaze patrolling with : " + col.name);
    //            behavior.ChangeState(EnemyStates.FOLLOW);
    //            //patrol.SecondaryTarget = col.transform;
    //        }
    //    }     
    //}
    //void OnTriggerExit(Collider col)
    //{
    //    if (col.CompareTag("Enemy") && col.GetType() == typeof(SphereCollider))
    //    {
    //        //if (stats.Target == null) {
    //        //    Debug.Log("Heading Back to Enemy");
    //        //    patrol.AutoPilot = true;
    //        //    autoTimer = 5f;
    //        //}
    //    }
    //}
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.CompareTag("Player") && detectionTimer <= 0f)
        {
            detectionTimer = 1f;
            if(explodeBot)
                hit.transform.SendMessage("Hit");
            else
                hit.transform.SendMessage("EMPHit");
            Explosion();
        }
    }
    #endregion
}