using UnityEngine;
using System.Collections;

public class PillarLighting : MonoBehaviour {

    public float timer;
    public float alphaTimer;
    private Material myMat;
    private Color lightColor;
    private GameObject player;


    // Use this for initialization
    void Start () {
        timer = 0.0f;
        alphaTimer = 1.0f;
        lightColor = Color.cyan;
        myMat = GetComponent<Renderer>().material;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update () {
        if (timer > 0)
            timer -= Time.deltaTime;        
        else {            
            if (Vector3.Distance(player.transform.position, transform.position) < 200.0f) {
                lightColor.a = 0.0f;
                myMat.SetColor("_TintColor", lightColor);
            }
            else
                MaterialShift();
        }
    }

    void MaterialShift() {
        if (alphaTimer > 0.0f) {
            alphaTimer -= 0.2f * Time.deltaTime;
            lightColor.a = alphaTimer;
            myMat.SetColor("_TintColor", lightColor);
        }
        else {
            alphaTimer = 1.0f;
            lightColor.a = alphaTimer;
            myMat.SetColor("_TintColor", lightColor);
            timer = 5.0f;
        }
    }
}
