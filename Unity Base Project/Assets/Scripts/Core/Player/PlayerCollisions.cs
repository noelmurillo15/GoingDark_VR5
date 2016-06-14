using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{

    private float padding;
    private PlayerMovement stats;
    private x360Controller controller;

    void Start()
    {
        padding = 0f;
        stats = GetComponent<PlayerMovement>();
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
            if (hit.transform.CompareTag("Asteroid"))
            {
                if (stats.GetMoveData().Speed > (stats.GetMoveData().MaxSpeed / 2f))
                    stats.SendMessage("Hit");
               
                stats.SendMessage("StopMovement");
            }
            padding = 5f;
        }
    }
}