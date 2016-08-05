using UnityEngine;

public class EnemyLaserCollision : MonoBehaviour
{

    #region Properties
    private LaserProjectile laser;
    #endregion

    // Use this for initialization
    void Start()
    {
        laser = GetComponent<LaserProjectile>();
    }

    #region Collisions
    void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("Asteroid"))
        {
            col.transform.SendMessage("Kill");
        }
        else if (col.transform.CompareTag("Player"))
        {
            col.transform.SendMessage("Kill");
            laser.Kill();
        }
    }
    #endregion
}