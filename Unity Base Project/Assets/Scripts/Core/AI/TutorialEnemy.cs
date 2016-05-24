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
    public void Kill()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
