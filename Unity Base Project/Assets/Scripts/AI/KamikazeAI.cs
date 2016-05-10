using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(CharacterController))]
public class KamikazeAI : MonoBehaviour {
    //**        Attach to Enemy     **//

    private float padding;
    //  Player Data        
    private Transform m_playerPos;

    //  Enemy Data
    private EnemyStats stats;
    private CharacterController controller;


    // Use this for initialization
    void Start()
    {
        padding = 0f;
        stats = GetComponent<EnemyStats>();
        controller = GetComponent<CharacterController>();
        m_playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (padding > 0f)
            padding -= Time.deltaTime;

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
        Vector3 newEnemyDir = Vector3.RotateTowards(transform.forward, playerDir, Time.deltaTime / 1.0f, 0.0f);
        transform.rotation = Quaternion.LookRotation(newEnemyDir);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.CompareTag("Player") && padding <= 0f)
        {
            padding = 1f;
            Debug.Log("Droid Has Hit");
            hit.transform.SendMessage("Hit");
            stats.SendMessage("Kill");
        }
    }
}