using UnityEngine;

public class Meteor : MonoBehaviour {

    // Use this for initialization
    private Vector3 velocity;
    private Transform myTransform;
    
	void Start () {
        myTransform = GetComponent<Transform>();
        velocity = new Vector3(Random.Range(-359, 359), Random.Range(-359, -1), Random.Range(-359, 359));
        // velocity = new Vector3(0, 0, -50);
        Invoke("Kill", Random.Range(15,20));
	}
	
	// Update is called once per frame
	void Update() {
                
        myTransform.Translate(velocity * Time.deltaTime);

	}

    void OnTriggerEnter(Collider col)
    {
        if (col.GetType() == typeof(MeshCollider) && col.transform.CompareTag("Missile"))
        {
            col.transform.SendMessage("Kill");
            Kill();
        }
        else if (col.transform.CompareTag("Player"))
        {
            AudioManager.instance.PlayHit();
            col.transform.SendMessage("Kill");
            Kill();
        }
        else if (col.transform.CompareTag("Enemy"))
        {
            col.transform.SendMessage("Kill");
            Kill();
        }
    }
    public void Kill()
    {
        Destroy(gameObject);
    }

    void OnBecameVisible()
    {
        enabled = true;
    }

    void OnBecameInvisible()
    {
        enabled = false;
    }
}
