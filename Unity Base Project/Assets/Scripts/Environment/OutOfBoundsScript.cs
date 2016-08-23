using UnityEngine;

public class OutOfBoundsScript : MonoBehaviour
{

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Enemy"))
            col.SendMessage("InBounds");
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Enemy"))
            col.SendMessage("OutOfBounds", transform.position);        
    }
}