using UnityEngine;

public class EnemyScript : MonoBehaviour {
    //**        Attach to Enemy Prefab      **//

    //  Player Data
    public float targetDist;
    private Cloak playerCloak;
    private Vector3 playerDir;
    private Transform m_playerPos;

    //  Enemy Data
    private float radius;
    private bool lockedOn;
    private int missileCount;
    private int maxMissileCount;
    private float missileCooldown;
    public GameObject missilePrefab;

    private GameObject messages;

    // Use this for initialization
    void Start()
    {
        lockedOn = false;
        radius = 250.0f;
        missileCount = 0;
        maxMissileCount = 2;
        missileCooldown = 5.0f;

        playerCloak = GameObject.Find("Cloak").GetComponent<Cloak>();
        m_playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        messages = GameObject.Find("Screen");

        playerDir = m_playerPos.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (missileCooldown > 0.0f)
            missileCooldown -= Time.deltaTime;

        LockOn();

        if (targetDist < radius)
            messages.SendMessage("EnemyClose");
        else
            messages.SendMessage("EnemyAway");

        if (lockedOn)
            Fire();
    }

    private void LockOn() {
        targetDist = Vector3.Distance(m_playerPos.position, transform.position);

        if (targetDist < radius && !playerCloak.GetCloaked())
        {
            playerDir = m_playerPos.position - transform.position;
            Vector3 newEnemyDir = Vector3.RotateTowards(transform.forward, playerDir, Time.deltaTime / 15.0f, 0.0f);
            transform.rotation = Quaternion.LookRotation(newEnemyDir);

            float angle = Vector3.Angle(newEnemyDir, playerDir);
            if (angle <= 8.0f)
                lockedOn = true;
        }
        else
            lockedOn = false;
    }

    private void Fire()
    {
        if (missileCount < maxMissileCount)
        {
            if (missileCooldown <= 0.0f)
            {
                missileCount++;
                missileCooldown = 10.0f;
                Instantiate(missilePrefab, this.transform.position, this.transform.rotation);
            }
        }        
    }

    public void EMPHit()
    {
        Debug.Log("EMP has affected Enemy's Systems");
    }

    public void AsteroidHit()
    {
        Debug.Log("Enemy Has Hit Asteroid");
    }

    public void Kill() {
        Debug.Log("Destroyed Enemy Ship");
        Destroy(this.gameObject);
    }
}