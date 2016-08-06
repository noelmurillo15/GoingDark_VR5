using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{

    private float padding;
    private PlayerStats stats;
    private PlayerMovement move;

    void Awake()
    {
        padding = 0f;
        stats = GetComponent<PlayerStats>();
        move = GetComponent<PlayerMovement>();
    }

    void LateUpdate()
    {
        if (padding > 0f)
            padding -= Time.deltaTime;
    }

    void OnCollisionEnter(Collision hit)
    {
        if (padding <= 0f)
        {
            if (hit.transform.CompareTag("Asteroid"))
            {
                stats.CrashHit(move.GetMoveData().Speed / move.GetMoveData().MaxSpeed);
                move.StopMovement();
            }
            if (hit.transform.CompareTag("Enemy"))
            {
                stats.CrashHit(move.GetMoveData().Speed / move.GetMoveData().MaxSpeed);
                move.StopMovement();
            }
            padding = 2f;
        }
    }
}