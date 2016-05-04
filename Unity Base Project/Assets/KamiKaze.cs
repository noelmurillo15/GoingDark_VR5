using UnityEngine;

public class KamiKaze : MonoBehaviour {
    //**        Attach to Enemy Kamikaze Prefab      **//

    //  Player Data
    public float targetDist;
    private Cloak playerCloak;
    private Vector3 playerDir;
    private Transform m_playerPos;

    //  Enemy Data
    private float velocity;
    private float radius;
    private bool inRange;

    public GameObject explode;
    private float deathTimer = 1.0f;
    private bool die = false;
    private float maxVelocity;
    private GameObject messages;

    // Use this for initialization
    void Start()
    {
        inRange = false;
        radius = 250.0f;
        velocity = 5.0f;
        maxVelocity = 80.0f;
        playerCloak = GameObject.Find("Cloak").GetComponent<Cloak>();
        m_playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        messages = GameObject.Find("Screen");
        // explode = Resources.Load("KamikazeExplode");

        playerDir = m_playerPos.position - transform.position;
    }

    // Update is called once per frame
    void Update() {
        if(!playerCloak.GetCloaked())
            DetermineRange();

        if (die) {
            deathTimer -= Time.deltaTime;
            if (deathTimer <= 0.0f)
                Destroy(gameObject);
        }
    }

    private void DetermineRange() {
        targetDist = Vector3.Distance(m_playerPos.position, transform.position);
        if (targetDist < radius) {
            if (!inRange) {
                messages.SendMessage("EnemyClose");
                inRange = true;
            }
            IncreaseVelocity();
            Chase();
            LockOn();
        }
        else {
            if (inRange) {
                messages.SendMessage("EnemyAway");
                inRange = false;
            }
            DecreaseVelocity();
        }
    }

    private void LockOn() {
        playerDir = m_playerPos.position - transform.position;
        Vector3 newEnemyDir = Vector3.RotateTowards(transform.forward, playerDir, Time.deltaTime / 4.0f, 0.0f);
        transform.rotation = Quaternion.LookRotation(newEnemyDir);            
    }

    private void IncreaseVelocity() {
        if (velocity < maxVelocity)
            velocity += Time.deltaTime * 3.0f;
    }

    private void DecreaseVelocity() {
        if (velocity > 0.0f)
            velocity -= Time.deltaTime * 5.0f;
        else
            velocity = 0.0f;
    }

    private void Chase() {
        transform.position += transform.forward * Time.deltaTime * velocity;
    }

    void OnCollisionEnter(Collision col) {
        if (col.transform.tag == "PlayerShip" && !die) {
            Debug.Log("Kamakazi has Kamakazied you");
            col.gameObject.GetComponentInChildren<PlayerShipData>().SendMessage("Hit");
            die = true;
            deathTimer = 0.25f;
            Instantiate(explode, transform.position, transform.rotation);
        }
    }
}