using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class BlastRadius : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Enemy"))
            col.SendMessage("SplashDmg");        

        if (col.CompareTag("Player"))
            col.SendMessage("SplashDmg");
    }
}
