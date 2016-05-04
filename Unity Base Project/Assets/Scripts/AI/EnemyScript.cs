using UnityEngine;

public class EnemyScript : MonoBehaviour {
    //**        Attach to Enemy     **//

    //  Player Data    
    private Vector3 playerDir;
    private Transform m_playerPos;

    //  Enemy Data
    private bool lockedOn;
    private int missileCount;
    private int maxMissileCount;
    public float velocity;
    private float maxVelocity;
    private float missileCooldown;
    public GameObject missilePrefab;

    

    // Use this for initialization
    void Start() {
        lockedOn = false;

        velocity = 0.0f;
        maxVelocity = 20.0f;

        missileCount = 0;
        maxMissileCount = 3;
        missileCooldown = 5.0f;
        
        
        m_playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        playerDir = m_playerPos.position - transform.position;
    }

    // Update is called once per frame
    void Update() {
        if (missileCooldown > 0.0f)
            missileCooldown -= Time.deltaTime;

        EliminatePlayer();               
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

    private void EliminatePlayer() {
        if(Vector3.Distance(transform.position, m_playerPos.position) < 100.0f) {
            Debug.Log("Enemy Decreasing Speed");
            DecreaseVelocity();
            LockOn();
            Chase();

            if (lockedOn)
                Fire();
        }
        else {
            Debug.Log("Enemy Increasing Speed");
            IncreaseVelocity();
            Chase();
            LockOn();      
        }
    }

    private void LockOn() {
        playerDir = m_playerPos.position - transform.position;
        Vector3 newEnemyDir = Vector3.RotateTowards(transform.forward, playerDir, Time.deltaTime / 1.5f, 0.0f);
        transform.rotation = Quaternion.LookRotation(newEnemyDir);

        float angle = Vector3.Angle(newEnemyDir, playerDir);

        if (angle <= 12.0f)
            lockedOn = true;
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
}