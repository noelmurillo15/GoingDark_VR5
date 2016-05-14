using UnityEngine;
using GD.Core.Enums;

public class CloakSystem : MonoBehaviour {

    #region Properties
    public bool Activated { get; private set; }
    public float Cooldown { get; private set; }

    private float padding;
    private float cloakTimer;
    private Color originalColor;
    private GameObject[] shipLights;

    private SystemsManager manager;
    #endregion

    // Use this for initialization
    void Start () {
        Activated = false;
        Cooldown = 0.0f;
        cloakTimer = 0.0f;        

        shipLights = new GameObject[5];
        GameObject parentLight = GameObject.Find("ShipLights");
        for (int x = 0; x < parentLight.transform.childCount; x++)
            shipLights[x] = parentLight.transform.GetChild(x).gameObject;

        originalColor = shipLights[0].GetComponent<Light>().color;
        manager = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemsManager>();
    }
	
	// Update is called once per frame
	void Update () {
        if (padding > 0.0f)
            padding -= Time.deltaTime;

        if (Cooldown > 0)
            Cooldown -= Time.deltaTime;

        if (cloakTimer > 0)
            cloakTimer -= Time.deltaTime;
        else if (cloakTimer < 0 && Activated)
            Activate(false);

        if (Input.GetKey(KeyCode.C))
            manager.ActivateSystem(SystemType.CLOAK);
    }

    #region Private Methods
    public void Activate(bool boolean) {
        if (padding <= 0.0f) {
            padding = 0.2f;
            if (boolean) {
                cloakTimer = 30.0f;
                for (int x = 0; x < shipLights.Length; x++)
                    shipLights[x].GetComponent<Light>().color = Color.black;
            }
            else {
                cloakTimer = 0.0f;
                Cooldown = 60.0f;
                for (int x = 0; x < shipLights.Length; x++)
                    shipLights[x].GetComponent<Light>().color = originalColor;
            }
            Activated = boolean;
        }
    }    
    #endregion
}