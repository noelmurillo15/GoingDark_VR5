using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    //**    Attach to Player Health Gameobject  **//
    private GameObject health1;
    private GameObject health2;
    private GameObject health3;
    private PlayerStats stats;


    // Use this for initialization
    void Start()
    {
        if (health1 == null)
            health1 = GameObject.Find("Health1");
        if (health2 == null)
            health2 = GameObject.Find("Health2");
        if (health3 == null)
            health3 = GameObject.Find("Health3");

        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        UpdatePlayerHealth();
    }

    // Update is called once per frame
    void Update() {

    }

    public void UpdatePlayerHealth() {
        switch (stats.GetHitCount()) {
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
