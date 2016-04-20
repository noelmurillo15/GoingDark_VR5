using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemyScript : MonoBehaviour
{

    //  Player Info
    public float zOffset;
    public GameObject m_Player;
    public PlayerData m_playerInput;
    public GameObject explosion;
    public GameObject[] Missiles;
    public GameObject Missile;
    public GameObject TargetPos;


    private float playerDistance;
    private float missileCooldown;
    private int missileCount;
    private int missileCountLimit;
    private Vector3 playerDir;
    // Use this for initialization
    void Start()
    {
        playerDistance = 0.0f;
        missileCooldown = 10.0f;
        missileCount = 0;
        missileCountLimit = 2;
        playerDir = m_Player.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (missileCooldown > 0.0f)
            missileCooldown -= Time.deltaTime;

        playerDistance = Vector3.Distance(m_Player.transform.position, transform.position);
        //Debug.Log("Distance to player is " + playerDistance);

        if (playerDistance <= 200.0f && !m_playerInput.GetCloaked())
        {
            // turn towards player
            playerDir = m_Player.transform.position - transform.position;
            Vector3 newEnemyDir = Vector3.RotateTowards(transform.forward, playerDir, Time.deltaTime / 10.0f, 0.0f);
            transform.rotation = Quaternion.LookRotation(newEnemyDir);

            float angle = Vector3.Angle(newEnemyDir, playerDir);
            if (angle <= 15.0f)
                Fire();
        }
    }

    void Fire()
    {
        if (missileCooldown <= 0.0f)
        {
            Missiles = GameObject.FindGameObjectsWithTag("Missile");

            if (missileCount <= missileCountLimit - 1)
            {
                missileCooldown = 10.0f;
                if (Missile != null)
                {
                    missileCount++;
                    Instantiate(Missile, new Vector3(transform.localPosition.x, transform.localPosition.y - 10.0f, transform.localPosition.z + 10.0f), transform.rotation);
                    Missile.GetComponent<EnemyMissile>().startTracking = true;
                    Debug.Log("Begin Missile Tracking");

                }
                else
                    Debug.Log("No Missile Gameobj attached");
            }
        }
    }

    void Kill()
    {
        Debug.Log("Destroyed Enemy Ship");
        Destroy(this.gameObject);
    }
}