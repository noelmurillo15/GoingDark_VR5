using UnityEngine;

public class DecoySystem : ShipDevice {

    #region Properties
    public int Count { get; private set; }

    private GameObject player;
    public GameObject decoy;
    #endregion

    // Use this for initialization
    void Start()
    {
        //Debug.Log("Initializing Decoy");
        Count = 5;
        maxCooldown = 10f;
        player = GameObject.FindGameObjectWithTag("Player");
        decoy = Resources.Load<GameObject>("Devices/DecoyShip");
    }

    // Update is called once per frame
    void Update()
    {
        if (Cooldown == maxCooldown)
        {
            Debug.Log("Decoy has been sent");
            Cooldown -= .01f;
            SendDecoy();
        }

        UpdateCooldown();
    }

    public void SendDecoy()
    {
        Count--;
        GameObject go = Instantiate(decoy, player.transform.position, player.transform.localRotation) as GameObject;
        go.transform.parent = player.transform.parent;
        go.SendMessage("SetSpeed", player.GetComponent<PlayerStats>().GetMoveData().Speed);
    }
}