using UnityEngine;

public class CloakSystem : ShipDevice
{

    #region Properties
    private float cloakTimer;
    private Color originalColor;
    private GameObject[] shipLights;
    #endregion

    // Use this for initialization
    void Start()
    {
        //Debug.Log("Initializing Cloak");
        maxCooldown = 60f;
        cloakTimer = 0f;
        shipLights = new GameObject[5];
        GameObject parentLight = GameObject.Find("ShipLights");
        for (int x = 0; x < parentLight.transform.childCount; x++)
            shipLights[x] = parentLight.transform.GetChild(x).gameObject;
        originalColor = shipLights[0].GetComponent<Light>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (cloakTimer > 0)
            cloakTimer -= Time.deltaTime;
        else
        {
            if(cloakTimer < 0f)
                UnCloakShip();
        }

        if (Cooldown == maxCooldown)
        {
            Debug.Log("Ship has been cloaked");
            CloakShip();
        }

        UpdateCooldown();
    }

    #region Private Methods
    public void CloakShip()
    {
        cloakTimer = 20.0f;
        for (int x = 0; x < shipLights.Length; x++)
            shipLights[x].GetComponent<Light>().color = Color.black;
    }

    public void UnCloakShip()
    {
        cloakTimer = 0f;
            for (int x = 0; x<shipLights.Length; x++)
                shipLights[x].GetComponent<Light>().color = originalColor;
    }
    #endregion
}