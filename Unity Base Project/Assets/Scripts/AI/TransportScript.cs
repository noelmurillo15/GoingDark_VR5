using UnityEngine;
using System.Collections;

public class TransportScript : MonoBehaviour 
{
    public GameObject loot;
    public GameObject child;
    private bool cloaked = false;
    public float cloakTimer = 0.0f;
    private float cloakCooldown = 0.0f;
	// Use this for initialization
	void Start () 
    {
        child = GameObject.Find("Explorer");
        loot = GameObject.Find("Loot");
        loot.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (cloakTimer > 0.0f)
        {
            cloakTimer -= Time.deltaTime;
            if (cloakTimer <= 0.0f)
                SetCloaked(false);
        }
        if (cloakCooldown > 0.0f)
            cloakCooldown -= Time.deltaTime;
	}

    bool GetCloaked()
    {
        return cloaked;
    }

    void SetCloaked(bool val)
    {
        if (cloakCooldown <= 0.0f && this.gameObject != null)
        {
            Color col = GetComponent<Renderer>().material.color;

            if (val && cloakTimer <= 0.0f)
            {
                col.a = 0.25f;
                cloakTimer = 30.0f;
            }
            else
            {
                col.a = 1.0f;
                cloakTimer = 0.0f;
                cloakCooldown = 25.0f;
            }
            cloaked = val;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player" && !col.GetComponent<PlayerData>().GetPlayerCloak().GetCloaked())
        {
            SetCloaked(true);
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player" && !col.GetComponent<PlayerData>().GetPlayerCloak().GetCloaked())
        {
            SetCloaked(true);
        }
    }


    void Kill()
    {
        Debug.Log("Destroyed Transport Ship");
        loot.SetActive(true);
        Destroy(child);
    }
}
