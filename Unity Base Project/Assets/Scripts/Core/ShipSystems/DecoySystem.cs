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
        Count = 5;
        maxCooldown = 10f;
        player = GameObject.FindGameObjectWithTag("Player");
        decoy = Resources.Load<GameObject>("Devices/DecoyShip");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
            Activate();

        if (Input.GetButtonDown("B"))
            Activate();

        if (Activated)
            SendDecoy();
    }

    public void SendDecoy()
    {
        if (Count > 0f)
        {
            Count--;
            DeActivate();
            GameObject go = Instantiate(decoy, player.transform.position, player.transform.localRotation) as GameObject;
            go.SendMessage("SetSpeed", player.GetComponent<PlayerMovement>().GetMoveData().Speed);
            go.transform.parent = player.transform.parent;
        }
    }
}