using UnityEngine;
using System.Collections;

public class DeathTimer : MonoBehaviour {
    private float ammoTimer;
    // Use this for initialization
    void Start () {
        ammoTimer = 20.0f;

    }
	
	// Update is called once per frame
	void Update () {
        if (ammoTimer > 0)
            ammoTimer -= Time.deltaTime;

        if (ammoTimer <= 0.0f)
        {
            Destroy(gameObject);
            ammoTimer = 20.0f;
        }
    }
}
