using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{

    private float padding;
    private PlayerStats stats;
    private PlayerMovement move;
    private x360Controller controller;

    void Start()
    {
        padding = 0f;
        stats = GetComponent<PlayerStats>();
        move = GetComponent<PlayerMovement>();
        controller = GamePadManager.Instance.GetController(0);
    }

    void Update()
    {
        if (padding > 0f)
            padding -= Time.deltaTime;
    }

    void OnCollisionEnter(Collision hit)
    {
        if (padding <= 0f)
        {
            if (hit.transform.CompareTag("Asteroid") || hit.transform.CompareTag("Enemy"))
            {
                if (move.GetMoveData().Speed > (move.GetMoveData().MaxSpeed / 2f))
                    move.SendMessage("Hit");

                stats.UnCloak();
                move.SendMessage("StopMovement");
            }
            padding = 5f;
        }
    }
}