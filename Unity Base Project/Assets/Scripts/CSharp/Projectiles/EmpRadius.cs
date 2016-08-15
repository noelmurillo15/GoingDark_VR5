using UnityEngine;

public class EmpRadius : MonoBehaviour
{

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Enemy"))
            col.SendMessage("EMPHit");

        if (col.CompareTag("Missile"))
            col.SendMessage("Kill");
    }
}
