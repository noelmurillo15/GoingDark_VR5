using UnityEngine;

public class EmpCollision : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Enemy") || col.CompareTag("TransportShip"))
            col.SendMessage("EMPHit");
    }
}
