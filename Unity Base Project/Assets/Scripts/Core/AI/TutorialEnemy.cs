using UnityEngine;
using System.Collections;

public class TutorialEnemy : MonoBehaviour {

    private GameObject explosion;
    public GameObject missilePrefab;
    private float missileCooldown;

    // Use this for initialization
    void Start () {
        explosion = Resources.Load<GameObject>("EnemyExplosion");
        missileCooldown = 0f;
    }

    // Update is called once per frame
    void Update () {
	
	}
    public void Hit()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    void ShieldHit()
    {
        Debug.Log("Enemy Shield Hit");
    }

    void OutOfBounds()
    {
    }
    void InBounds()
    {
    }

    public void Fire(float delay)
    {
        if (missilePrefab == null)
        {
            missilePrefab = Resources.Load<GameObject>("Missiles/EnemyMissile");
        }

        missileCooldown += Time.deltaTime;
        if (missileCooldown >= delay)
        {
            missileCooldown = 0.0f;
            Instantiate(missilePrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        }

    }
}
