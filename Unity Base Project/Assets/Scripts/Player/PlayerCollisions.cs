using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerCollisions : MonoBehaviour {

    private float padding;
    private PlayerStats stats;

    void Start()
    {
        padding = 0f;
        stats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (padding > 0f)
            padding -= Time.deltaTime;
    }
        
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (padding <= 0f)
        {
            if (hit.transform.CompareTag("Asteroid"))
            {
                if(stats.GetMoveSpeed() > (stats.GetMaxSpeed() / 2f))
                    stats.SendMessage("Hit");

                stats.SendMessage("StopMovement");
            }
            padding = 1f;
        }
    }
}