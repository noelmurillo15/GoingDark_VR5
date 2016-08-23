using UnityEngine;

public class CloakSystem : ShipSystem
{
    #region Properties
    private bool isCloaked;
    private Color originalColor;
    private GameObject[] shipLights;
    #endregion

    // Use this for initialization
    void Start()
    {
        maxCooldown = 30f;

        shipLights = new GameObject[4];
        GameObject parentLight = GameObject.Find("ShipLights");
        for (int x = 0; x < parentLight.transform.childCount; x++)
            shipLights[x] = parentLight.transform.GetChild(x).gameObject;
        originalColor = shipLights[0].GetComponent<Light>().color;
    }

    void Update()
    {
        if (cooldown > 0f)
            cooldown -= Time.deltaTime;

        if (Activated)
            CloakShip();
    }

    public bool GetCloaked()
    {
        return isCloaked;
    }

    void CloakShip()
    {
        AudioManager.instance.PlayCloak();
        isCloaked = true;
        for (int x = 0; x < shipLights.Length; x++)
            shipLights[x].GetComponent<Light>().color = Color.black;

        Activated = false;
    }

    public void UnCloakShip()
    {
        isCloaked = false;
        AudioManager.instance.PlayCloak();
        for (int x = 0; x < shipLights.Length; x++)
            shipLights[x].GetComponent<Light>().color = originalColor;

        DeActivate();
    }
}