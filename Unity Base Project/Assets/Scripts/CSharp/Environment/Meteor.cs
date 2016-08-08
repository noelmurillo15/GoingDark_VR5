using UnityEngine;

public class Meteor : MonoBehaviour {

    // Use this for initialization
    private Vector3 velocity;
    private Transform MyTransform;

    void Start()
    {
        MyTransform = transform;
        velocity = new Vector3(Random.Range(-1, 1), -1f, Random.Range(-1, 1));
        velocity *= Random.Range(20f, 50f) * 20f;
        Invoke("Kill", 30f);
    }
	
	// Update is called once per frame
	void FixedUpdate() {                
        MyTransform.Translate(velocity * Time.fixedDeltaTime);
	}
    public void Kill()
    {
        AudioManager.instance.PlayHit();
        Destroy(gameObject);
    }
}