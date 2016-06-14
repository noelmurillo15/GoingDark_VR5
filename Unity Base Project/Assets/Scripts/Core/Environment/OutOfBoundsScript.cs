using UnityEngine;

public class OutOfBoundsScript : MonoBehaviour
{

    private GameObject messages;
    private GameObject Player;
    private float DisplayDuration;

    // Use this for initialization
    void Start()
    {
        messages = GameObject.Find("PlayerCanvas");
        DisplayDuration = 10.0f;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            messages.SendMessage("ManualPilot");
            col.SendMessage("InBounds");
            if (IsInvoking("EnableAutoPilot"))
                CancelInvoke("EnableAutoPilot");
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Player = col.gameObject;
            SendWarning();
            Invoke("EnableAutoPilot", DisplayDuration);
        }
    }
    void SendWarning()
    {
        messages.SendMessage("AutoPilot");
    }

    void EnableAutoPilot()
    {
        Player.SendMessage("OutOfBounds", Vector3.zero);
    }
}