using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemyScript : MonoBehaviour {
    //**        Attach To Enemy Prefab        **//

    //  Player Info
    public float zOffset;
    public GameObject m_Player;
    public PlayerData m_playerInput;
    public GameObject explosion;

    private float playerDistance;
    private Vector3 playerDir;
    // Use this for initialization
    void Start()
    {
        playerDistance = 0.0f;
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_playerInput = m_Player.GetComponent<PlayerData>();
        playerDir = m_Player.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update() {
        playerDistance = Vector3.Distance(m_Player.transform.position, transform.position);

        if (playerDistance <= 200.0f)
        {
            // turn towards player
            playerDir = m_Player.transform.position - transform.position;
            Vector3 newEnemyDir = Vector3.RotateTowards(transform.forward, playerDir, Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newEnemyDir);
        }
    }

    void Fire()
    {

    }

    void Kill()
    {
        Debug.Log("Destroyed Enemy Ship");
        Destroy(this.gameObject);
    }
}