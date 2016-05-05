using UnityEngine;
using System.Collections;

public class Death : MonoBehaviour
{

    void OnTriggerEnter(Collider col)
    {    
        col.SendMessage("Kill");
    }
}
