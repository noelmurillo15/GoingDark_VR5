using UnityEngine;

public class OutOfBoundsScript : MonoBehaviour
{

    private GameObject messages;

    // Use this for initialization
    void Start()
    {
        messages = GameObject.Find("WarningMessages");
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            messages.SendMessage("ManualPilot");
            col.SendMessage("InBounds");
        }

        if (col.CompareTag("Enemy") || col.CompareTag("TransportShip"))
            if (col.GetType() == typeof(CharacterController))
                col.SendMessage("InBounds");
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            messages.SendMessage("AutoPilot");
            col.SendMessage("OutOfBounds", Vector3.zero);
        }

        if (col.CompareTag("Enemy") || col.CompareTag("TransportShip"))
            if (col.GetType() == typeof(CharacterController))
                col.SendMessage("OutOfBounds");
    }
}