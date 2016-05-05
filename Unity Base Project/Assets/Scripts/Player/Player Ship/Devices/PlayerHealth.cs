using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
    //**    Attach to Player Health Gameobject  **//
    public float autoRepairTimer;
    private GameObject health1;
    private GameObject health2;
    private GameObject health3;
    private PlayerShipData player;


    // Use this for initialization
    void Start()
    {
        autoRepairTimer = 0.0f;

        if (health1 == null)
            health1 = GameObject.Find("Health1");
        if (health2 == null)
            health2 = GameObject.Find("Health2");
        if (health3 == null)
            health3 = GameObject.Find("Health3");

        player = GameObject.Find("BattleShip").GetComponent<PlayerShipData>();
        UpdatePlayerHealth();
    }

    // Update is called once per frame
    void Update() {
        if (autoRepairTimer > 0.0f)
            autoRepairTimer -= Time.deltaTime;
        else {
            if (player.GetHitCount() != 0) {
                Debug.Log("Player HP Regenerated");
                player.DecreaseHitCount();
                UpdatePlayerHealth();
            }
            autoRepairTimer = 120.0f;
        }
    }

    public void UpdatePlayerHealth() {
        switch (player.GetHitCount()) {
            case 1:
                health1.gameObject.GetComponent<Renderer>().material.color = Color.red;
                health2.gameObject.GetComponent<Renderer>().material.color = Color.green;
                health3.gameObject.GetComponent<Renderer>().material.color = Color.green;
                break;
            case 2:
                health1.gameObject.GetComponent<Renderer>().material.color = Color.red;
                health2.gameObject.GetComponent<Renderer>().material.color = Color.red;
                health3.gameObject.GetComponent<Renderer>().material.color = Color.green;
                break;
            case 3:
                health1.gameObject.GetComponent<Renderer>().material.color = Color.red;
                health2.gameObject.GetComponent<Renderer>().material.color = Color.red;
                health3.gameObject.GetComponent<Renderer>().material.color = Color.red;
                break;            
            default:
                health1.gameObject.GetComponent<Renderer>().material.color = Color.green;
                health2.gameObject.GetComponent<Renderer>().material.color = Color.green;
                health3.gameObject.GetComponent<Renderer>().material.color = Color.green;
                break;
        }      
    }
}
