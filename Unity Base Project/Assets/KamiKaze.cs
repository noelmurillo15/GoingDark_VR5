using UnityEngine;

public class KamiKaze : MonoBehaviour
{
    //**        Attach to Enemy Kamikaze Prefab      **//

    //  Player Data
    public float targetDist;
    private Cloak playerCloak;
    private Vector3 playerDir;
    private Transform m_playerPos;

    //  Enemy Data
    private float radius;
    private bool inRange;
    private bool lockedOn;

    public GameObject explode;
    private float deathTimer = 1.0f;
    private bool die = false;

    private GameObject messages;

    // Use this for initialization
    void Start()
    {
        inRange = false;
        lockedOn = false;
        radius = 250.0f;

        playerCloak = GameObject.Find("Cloak").GetComponent<Cloak>();
        m_playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        messages = GameObject.Find("Screen");
        // explode = Resources.Load("KamikazeExplode");
        
        playerDir = m_playerPos.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        DetermineRange();
        LockOn();
        if (targetDist <= 100 && lockedOn)
        {
            Chase();
        }

        if(die)
        {
            deathTimer -= Time.deltaTime;
            if (deathTimer <= 0.0f)
                Destroy(gameObject);
        }
    }

    private void DetermineRange()
    {
        targetDist = Vector3.Distance(m_playerPos.position, transform.position);
        if (targetDist < radius)
        {
            if (!inRange)
                messages.SendMessage("EnemyClose");
            inRange = true;
        }
        else
        {
            if (inRange)
                messages.SendMessage("EnemyAway");
            inRange = false;
        }
    }

    private void LockOn()
    {
        if (inRange)
        {
            if (!playerCloak.GetCloaked())
            {
                playerDir = m_playerPos.position - transform.position;
                Vector3 newEnemyDir = Vector3.RotateTowards(transform.forward, playerDir, Time.deltaTime / 4.0f, 0.0f);
                transform.rotation = Quaternion.LookRotation(newEnemyDir);

                float angle = Vector3.Angle(newEnemyDir, playerDir);
                if (angle <= 8.0f)
                    lockedOn = true;
            }
        }
        else
            lockedOn = false;
    }
    private void Chase()
    {
        transform.position += transform.forward * Time.deltaTime * 4;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "PlayerShip")
        {
            Debug.Log("Kamakazi has Kamakazied you");
            col.gameObject.GetComponentInChildren<PlayerShipData>().SendMessage("Hit");
            die = true;
            deathTimer = 0.25f;
            Instantiate(explode, transform.position, transform.rotation);
        }
    }
}