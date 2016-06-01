using UnityEngine;
using System.Collections;

public class TutorialEnemy : MonoBehaviour {

    private GameObject explosion;

	// Use this for initialization
	void Start () {
        explosion = Resources.Load<GameObject>("EnemyExplosion");
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
}
