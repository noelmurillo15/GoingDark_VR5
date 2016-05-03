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
    private bool inRange;
    private bool lockedOn;
    private int missileCount;
    private int maxMissileCount;
    public float velocity;
    private float maxVelocity;
    private float missileCooldown;
    public GameObject missilePrefab;

    private GameObject messages;

    // Use this for initialization
    void Start() {
        inRange = false;
        lockedOn = false;

        velocity = 0.0f;
        maxVelocity = 20.0f;

        radius = 250.0f;
        missileCount = 0;
        maxMissileCount = 3;
        missileCooldown = 5.0f;

        messages = GameObject.Find("Screen");
        playerCloak = GameObject.Find("Cloak").GetComponent<Cloak>();
        m_playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        playerDir = m_playerPos.position - transform.position;
    }

    // Update is called once per frame
    void Update() {
        if (missileCooldown > 0.0f)
            missileCooldown -= Time.deltaTime;

        if(!playerCloak.GetCloaked())
            DetermineRange();               
    }

    private void IncreaseVelocity() {
        if (velocity < maxVelocity)
            velocity += Time.deltaTime * 2.0f;
    }

    private void DecreaseVelocity() {
        if (velocity > 0.0f)
            velocity -= Time.deltaTime * 6.0f;
        else
            velocity = 0.0f;
    }

    private void Chase() {
        transform.position += transform.forward * Time.deltaTime * velocity;
    }

    private void DetermineRange() {
        targetDist = Vector3.Distance(m_playerPos.position, transform.position);
        if (targetDist < radius) {

            if (!inRange)
                messages.SendMessage("EnemyClose");

            if (targetDist < 100.0f)
            {
                DecreaseVelocity();

                if(lockedOn)
                    Fire();
            }
            else
                IncreaseVelocity();

            Chase();
            LockOn();
            inRange = true;
        }
        else {
            if (inRange)
                messages.SendMessage("EnemyAway");

            DecreaseVelocity();          
            inRange = false;
        }
    }

    private void LockOn() {
        playerDir = m_playerPos.position - transform.position;
        Vector3 newEnemyDir = Vector3.RotateTowards(transform.forward, playerDir, Time.deltaTime / 4.0f, 0.0f);
        transform.rotation = Quaternion.LookRotation(newEnemyDir);

        float angle = Vector3.Angle(newEnemyDir, playerDir);

        if (angle <= 15.0f)
            lockedOn = true;
        else
            lockedOn = false;
    }      

    private void Fire() {
        if (missileCount < maxMissileCount) {
            if (missileCooldown <= 0.0f) {
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

    public void Kill()
    {
        Debug.Log("Destroyed Enemy Ship");
        Destroy(this.gameObject);
    }
}