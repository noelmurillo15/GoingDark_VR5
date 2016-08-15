using UnityEngine;

public class BlastRadius : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Enemy"))
            col.SendMessage("SplashDmg");        

        if (col.CompareTag("Player"))
            col.SendMessage("SplashDmg");        
    }
}
