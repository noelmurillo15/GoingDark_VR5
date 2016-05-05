using UnityEngine;
using System.Collections;

public class Death : MonoBehaviour
{

    void OnTriggerEnter(Collider col)
    {    
        if(col.gameObject.tag == "Player" || col.gameObject.tag == "Enemy" || col.gameObject.tag == "Asteroid" || col.gameObject.tag == "TransportShip")
            col.SendMessage("Kill");
    }
}
