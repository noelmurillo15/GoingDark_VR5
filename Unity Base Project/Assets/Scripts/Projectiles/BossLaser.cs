using UnityEngine;
using GoingDark.Core.Enums;

public class BossLaser : MonoBehaviour {

    [SerializeField]
    private GameObject Laser;
    
    private ObjectPoolManager PoolManager;

    // Use this for initialization
    void Start () {
        Laser.SetActive(false);
        InvokeRepeating("Attack", 5f, 20f);
        PoolManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ObjectPoolManager>();                
	}

    void Attack()
    {
        Laser.SetActive(true);
        Invoke("Fire", 4.5f);
        Invoke("Cooldown", 6f);
    }

    void Fire()
    {
        GameObject obj1 = PoolManager.GetLaser(EnemyLaserType.Cannon);

        if (obj1 != null)
        {
            obj1.transform.position = Laser.transform.position;
            obj1.transform.rotation = Laser.transform.rotation;
            obj1.SetActive(true);
        }
    }

    void Cooldown()
    {
        Laser.SetActive(false);
    }
}
