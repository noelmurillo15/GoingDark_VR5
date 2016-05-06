using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
public class EnemyAttack : MonoBehaviour {
    //**        Attach to Enemy     **//

    //  Player Data        
    private Transform m_playerPos;

    //  Missile Data
    private bool lockedOn;
    private float missileCooldown;
    public GameObject missilePrefab;

    //  Enemy Data
    private EnemyStats stats;


    // Use this for initialization
    void Start() {
        lockedOn = false;
        missileCooldown = 5.0f;
        stats = GetComponent<EnemyStats>();
        m_playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update() {
        if (missileCooldown > 0.0f)
            missileCooldown -= Time.deltaTime;

        EliminatePlayer();               
    }

    private void Chase() {
        transform.Translate(transform.forward * Time.deltaTime * stats.GetMoveSpeed());
    }

    private void EliminatePlayer() {
        if(Vector3.Distance(transform.position, m_playerPos.position) < 360.0f) {
            stats.DecreaseSpeed();
            if (lockedOn)
                Fire();
        }
        else 
            stats.IncreaseSpeed(0.8f);            

        Chase();
        LockOn();
    }

    private void LockOn() {
        Vector3 playerDir = m_playerPos.position - transform.position;
        Vector3 newEnemyDir = Vector3.RotateTowards(transform.forward, playerDir, Time.deltaTime / 2.0f, 0.0f);
        transform.rotation = Quaternion.LookRotation(newEnemyDir);

        float angle = Vector3.Angle(newEnemyDir, playerDir);

        if (angle <= 8.0f)
            lockedOn = true;
        else
            lockedOn = false;
    }

    private void Fire() {
        if (stats.GetNumMissiles() > 0) {
            if (missileCooldown <= 0.0f) {
                missileCooldown = 10.0f;
                stats.DecreaseMissileCount();
                Instantiate(missilePrefab, this.transform.position, this.transform.rotation);
            }
        }
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.CompareTag("Player"))
        {
            hit.transform.FindChild("BattleShip").SendMessage("Hit");
        }
    }
}