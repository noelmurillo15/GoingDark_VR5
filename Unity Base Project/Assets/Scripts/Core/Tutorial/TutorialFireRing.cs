using UnityEngine;
using System.Collections;

public class TutorialFireRing : MonoBehaviour {
    TutorialFlight tutorial;
    public GameObject prevRing;
    private BoxCollider box;
    private bool buffer;
    private GameObject particle;
	// Use this for initialization
	void Start () {
        tutorial = GameObject.Find("TutorialPrefF").GetComponent<TutorialFlight>();
        box = GetComponent<BoxCollider>();
        buffer = false;
        particle = transform.FindChild("Particles").gameObject;
        particle.SetActive(false);
        box.enabled = false;
      
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!prevRing && !buffer)
        {
            buffer = true;
            box.enabled = true;
            particle.SetActive(true);
        }
	}

    void OnBecameVisible()
    {
        enabled = true;
    }

    void OnBecameInvisible()
    {
        enabled = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            AudioManager.instance.PlayCollect();
            tutorial.SendMessage("AddRingCount");
            Destroy(gameObject);
        }
    }

}
