using UnityEngine;
using UnityEngine.SceneManagement;

public class OutOfBoundsScript : MonoBehaviour {

    public bool danger;
    public float timeToReturn;

	// Use this for initialization
	void Start () {
        danger = false;
        timeToReturn = 30.0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (danger)
            timeToReturn -= Time.deltaTime;

        if (timeToReturn <= 0.0f && danger)
            SceneManager.LoadScene("Credits_Scene");
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player") {
            Debug.Log("In Bounds");
            col.SendMessage("InBounds");
            timeToReturn = 0.0f;
            danger = false;
        }

        if (col.tag == "Asteroid")
        {
            Debug.Log("Asteroid Detected");
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            Debug.Log("Out of Bounds");
            Vector3 CenterOfWorld = this.transform.position;
            CenterOfWorld.z = 2000.0f;
            col.SendMessage("OutOfBounds", CenterOfWorld);
            timeToReturn = 30.0f;
            danger = true;
        }
    }
}