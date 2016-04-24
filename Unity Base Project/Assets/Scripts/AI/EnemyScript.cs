using UnityEngine;

public class EnemyScript : MonoBehaviour {
    //**        Attach to Enemy Prefab      **//

    //  Player Data
    private Cloak playerCloak;
    private Vector3 playerDir;
    public Transform m_playerPos;

    //  Enemy Data
    private int missileCount;
    private int maxMissileCount;
    private float missileCooldown;
    public GameObject missilePrefab;


    // Use this for initialization
    void Start()
    {
        missileCount = 0;
        maxMissileCount = 2;
        missileCooldown = 10.0f;

        playerCloak = GameObject.Find("Cloak").GetComponent<Cloak>();

        m_playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        playerDir = m_playerPos.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (missileCooldown > 0.0f)
            missileCooldown -= Time.deltaTime;

        float targetDistance = Vector3.Distance(m_playerPos.position, transform.position);

        if (targetDistance <= 125.0f && !playerCloak.GetCloaked())  {
            // turn towards player
            playerDir = m_playerPos.position - transform.position;
            Vector3 newEnemyDir = Vector3.RotateTowards(transform.forward, playerDir, Time.deltaTime / 5.0f, 0.0f);
            transform.rotation = Quaternion.LookRotation(newEnemyDir);

            float angle = Vector3.Angle(newEnemyDir, playerDir);
            if (angle <= 8.0f)
                Fire();
        }
    }

    void Fire()
    {        
        if (missileCooldown <= 0.0f)
            if (missileCount < maxMissileCount)
                if (missilePrefab != null) {
                    missileCount++;
                    missileCooldown = 10.0f;
                    Instantiate(missilePrefab, new Vector3(transform.localPosition.x, transform.localPosition.y - 10.0f, transform.localPosition.z + 10.0f), transform.rotation);
                }
                else
                    Debug.Log("Enemy Missile Not Attached");                   
    }

    void Kill() {
        Debug.Log("Destroyed Enemy Ship");
        Destroy(this.gameObject);
    }
}