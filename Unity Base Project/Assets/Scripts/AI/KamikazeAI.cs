using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(CharacterController))]
public class KamikazeAI : MonoBehaviour {
    //**        Attach to Enemy     **//

    //  Player Data        
    private Transform m_playerPos;

    //  Enemy Data
    private EnemyStats stats;
    private CharacterController controller;


    // Use this for initialization
    void Start()
    {
        stats = GetComponent<EnemyStats>();
        controller = GetComponent<CharacterController>();
        m_playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

        EliminatePlayer();
    }

    private void Chase()
    {
        controller.Move(transform.forward * Time.deltaTime * stats.GetMoveSpeed());
    }

    private void EliminatePlayer()
    {    
        stats.IncreaseSpeed(1f);
        Chase();
        LockOn();
    }

    private void LockOn()
    {
        Vector3 playerDir = m_playerPos.position - transform.position;
        Vector3 newEnemyDir = Vector3.RotateTowards(transform.forward, playerDir, Time.deltaTime / 2.0f, 0.0f);
        transform.rotation = Quaternion.LookRotation(newEnemyDir);
    }
}