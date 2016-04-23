using UnityEngine;
using System.Collections;

public class Cloak : MonoBehaviour {

    private float padding;

    public bool isCloaked;
    public float cloakTimer;
    public float cloakCooldown;


    // Use this for initialization
    void Start () {
        isCloaked = false;
        cloakTimer = 0.0f;
        cloakCooldown = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
        if (cloakCooldown > 0)
            cloakCooldown -= Time.deltaTime;

        if (cloakTimer > 0)
            cloakTimer -= Time.deltaTime;
        else if (cloakTimer < 0)
            SetCloaked(false);
    }

    public void SetCloaked(bool boolean)
    {
        Debug.Log("Setting cloak to : " + boolean);
        if (boolean)
        {
            cloakTimer = 30.0f;
            //for (int x = 0; x < shipLights.Length; x++)
            //    shipLights[x].gameObject.SetActive(false);
        }
        else
        {
            cloakTimer = 0.0f;
            cloakCooldown = 60.0f;
            //for (int x = 0; x < shipLights.Length; x++)
            //    shipLights[x].gameObject.SetActive(true);
        }
        isCloaked = boolean;
    }

    public bool GetCloaked()
    {
        return isCloaked;
    }
    public float GetCloakTimer()
    {
        return cloakTimer;
    }
    public float GetCloakCooldown()
    {
        return cloakCooldown;
    }    
}
