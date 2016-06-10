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
        cloakTimer = 20;
        shipLights = new GameObject[5];
        GameObject parentLight = GameObject.Find("ShipLights");
        for (int x = 0; x < parentLight.transform.childCount; x++)
            shipLights[x] = parentLight.transform.GetChild(x).gameObject;
        originalColor = shipLights[0].GetComponent<Light>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("X"))
            Activate();

        if (Activated)
            CloakShip();
    }

    public bool GetCloaked()
    {
        return isCloaked;
    }

    public float GetCloakTimer()
    {
        return cloakTimer;
    }

    #region Private Methods
    void CloakShip()
    {
        AudioManager.instance.PlayCloak();
        isCloaked = true;
        for (int x = 0; x < shipLights.Length; x++)
            shipLights[x].GetComponent<Light>().color = Color.black;

        DeActivate();
        Invoke("UnCloakShip", cloakTimer);
    }

    void UnCloakShip()
    {
        isCloaked = false;
        AudioManager.instance.PlayCloak();
        for (int x = 0; x < shipLights.Length; x++)
            shipLights[x].GetComponent<Light>().color = originalColor;
    }
    #endregion
}