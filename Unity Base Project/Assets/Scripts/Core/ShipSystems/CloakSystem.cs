using UnityEngine;

public class CloakSystem : ShipDevice
{

    #region Properties
    private bool isCloaked;
    private float cloakTimer;
    private Color originalColor;
    private GameObject[] shipLights;
    #endregion

    // Use this for initialization
    void Start()
    {
        maxCooldown = 60f;
        cloakTimer = 10;
        shipLights = new GameObject[5];
        GameObject parentLight = GameObject.Find("ShipLights");
        for (int x = 0; x < parentLight.transform.childCount; x++)
            shipLights[x] = parentLight.transform.GetChild(x).gameObject;
        originalColor = shipLights[0].GetComponent<Light>().color;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.C))
            Activate();

        if (Activated)
            CloakShip();
    }

    public bool GetPlayerCloaked()
    {
        return isCloaked;
    }

    #region Private Methods
    void CloakShip()
    {
        AudioManager.instance.PlayCloak();
        for (int x = 0; x < shipLights.Length; x++)
            shipLights[x].GetComponent<Light>().color = Color.black;

        DeActivate();
        Invoke("UnCloakShip", cloakTimer);
    }

    void UnCloakShip()
    {
        AudioManager.instance.PlayCloak();
        for (int x = 0; x < shipLights.Length; x++)
            shipLights[x].GetComponent<Light>().color = originalColor;
    }
    #endregion
}