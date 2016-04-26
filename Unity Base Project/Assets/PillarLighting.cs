using UnityEngine;
using System.Collections;

public class PillarLighting : MonoBehaviour {

    public float timer;
    public float alphaTimer;
    private Color lightColor;
    private GameObject player;



    // Use this for initialization
    void Start () {
        timer = 20.0f;

        if (transform.parent.tag == "Loot")
            lightColor = Color.cyan;
        else
            lightColor = Color.yellow;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update () {
        if (timer > 0)
        {
            alphaTimer = 5.0f;
            timer -= 5.0f * Time.deltaTime;
        }
        else
        {            
            if (Vector3.Distance(player.transform.position, transform.position) < 150.0f)
            {
                Debug.Log("Reset Alpha");
                lightColor.a = 0.0f;
                GetComponent<Renderer>().material.SetColor("_TintColor", lightColor);
            }
            else
                MaterialShift();
        }
    }

    void MaterialShift()
    {
        if (alphaTimer > 0.0f)
        {
            Debug.Log("Changing Alpha");
            alphaTimer -= 0.75f * Time.deltaTime;
            lightColor.a = alphaTimer;
            GetComponent<Renderer>().material.SetColor("_TintColor", lightColor);
        }
        else
        {
            Debug.Log("Reset Alpha");
            lightColor.a = 1.0f;
            GetComponent<Renderer>().material.SetColor("_TintColor", lightColor);
            alphaTimer = 1.0f;
            timer = 20.0f;
        }
    }
}
