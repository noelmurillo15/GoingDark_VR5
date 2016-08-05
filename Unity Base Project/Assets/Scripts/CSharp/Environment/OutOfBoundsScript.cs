using UnityEngine;

public class OutOfBoundsScript : MonoBehaviour
{

    private GameObject messages;
    private GameObject Player;
    private float DisplayDuration;

    // Use this for initialization
    void Start()
    {
        DisplayDuration = 10.0f;
        messages = GameObject.Find("PlayerCanvas");
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Enemy"))
        {
            Debug.Log("Enemy Out of Bounds");
            col.SendMessage("OutOfBounds", transform.position);
        }    
    }

    void SendWarning()
    {
        messages.SendMessage("AutoPilot");
    }
    void DisableWarning()
    {
        messages.SendMessage("ManualPilot");
    }

    void EnableAutoPilot()
    {
        Player.SendMessage("OutOfBounds", Vector3.zero);
    }
}