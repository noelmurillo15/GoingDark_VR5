using UnityEngine;

public class Meteor : MonoBehaviour {

    // Use this for initialization
    private Vector3 velocity;
    private Transform myTransform;
    
	void Start () {
        myTransform = GetComponent<Transform>();
        Invoke("Kill", Random.Range(20,30));
        velocity = new Vector3(Random.Range(-359, 359), Random.Range(-359, -1), Random.Range(-359, 359));
	}
	
	// Update is called once per frame
	void FixedUpdate() {                
        myTransform.Translate(velocity * Time.fixedDeltaTime);
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.CompareTag("Player"))
        {
            AudioManager.instance.PlayHit();
            col.transform.SendMessage("Kill");
        }
        else if (col.transform.CompareTag("Enemy"))
        {
            col.transform.SendMessage("Kill");
        }
    }
    public void Kill()
    {
        Destroy(gameObject);
    }
}
