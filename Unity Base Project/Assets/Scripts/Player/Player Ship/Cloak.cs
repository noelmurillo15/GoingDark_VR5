using UnityEngine;
using System.Collections;

public class Cloak : MonoBehaviour {
    //**    Attach to Cloak GameObject  **//
    private float padding;

    private bool isCloaked;
    private float cloakTimer;
    private float cloakCooldown;

    private Color originalColor;
    private GameObject[] shipLights;


    // Use this for initialization
    void Start () {
        isCloaked = false;
        cloakTimer = 0.0f;
        cloakCooldown = 0.0f;

        shipLights = new GameObject[5];
        GameObject parentLight = GameObject.Find("ShipLights");
        for (int x = 0; x < parentLight.transform.childCount; x++)
            shipLights[x] = parentLight.transform.GetChild(x).gameObject;

        originalColor = shipLights[0].GetComponent<Light>().color;
    }
	
	// Update is called once per frame
	void Update () {
        if (padding > 0.0f)
            padding -= Time.deltaTime;

        if (cloakCooldown > 0)
            cloakCooldown -= Time.deltaTime;

        if (cloakTimer > 0)
            cloakTimer -= Time.deltaTime;
        else if (cloakTimer < 0 && GetCloaked())
            SetCloaked(false);
    }

    #region Modifiers
    public void SetCloaked(bool boolean) {
        if (padding <= 0.0f) {
            padding = 0.2f;
            if (boolean) {
                cloakTimer = 30.0f;
                for (int x = 0; x < shipLights.Length; x++)
                    shipLights[x].GetComponent<Light>().color = Color.black;
            }
            else {
                cloakTimer = 0.0f;
                cloakCooldown = 60.0f;
                for (int x = 0; x < shipLights.Length; x++)
                    shipLights[x].GetComponent<Light>().color = originalColor;
            }
            isCloaked = boolean;
        }
    }
    #endregion

    #region Accessors
    public bool GetCloaked() {
        return isCloaked;
    }

    public float GetCloakTimer() {
        return cloakTimer;
    }
    public float GetCloakCooldown() {
        return cloakCooldown;
    }
    #endregion
}
