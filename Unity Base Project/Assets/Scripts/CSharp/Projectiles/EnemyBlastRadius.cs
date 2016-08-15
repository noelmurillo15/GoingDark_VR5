using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemyBlastRadius : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
            col.SendMessage("SplashDmg");
    }
}
