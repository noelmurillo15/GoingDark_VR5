using UnityEngine;

public class DecoySystem : ShipDevice
{

    #region Properties
    public int Count { get; private set; }

    private GameObject cam;
    public GameObject decoy;
    #endregion

    // Use this for initialization
    void Start()
    {
        Count = 5;
        maxCooldown = 10f;
        cam = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Cooldown == maxCooldown)
        {
            Debug.Log("Decoy has been activated");
            SendDecoy();
        }

        UpdateCooldown();
    }

    public void SendDecoy()
    {
        Count--;
        GameObject go = Instantiate(decoy, cam.transform.position, cam.transform.localRotation) as GameObject;
        go.transform.parent = transform;
    }
}