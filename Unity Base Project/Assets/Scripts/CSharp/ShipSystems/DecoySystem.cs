using UnityEngine;

public class DecoySystem : ShipSystem {

    #region Properties
    public int Count { get; private set; }

    private Transform player;
    public GameObject decoy;
    #endregion

    // Use this for initialization
    void Start()
    {
        Count = 5;
        maxCooldown = 10f;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        decoy = Resources.Load<GameObject>("Devices/DecoyShip");
    }

    // Update is called once per frame
    void Update()
    {
        if (Activated)
            SendDecoy();

        if (cooldown > 0f)
            cooldown -= Time.deltaTime;
    }

    public void SendDecoy()
    {
        if (Count > 0f)
        {
            Count--;
            DeActivate();
            GameObject go = Instantiate(decoy, new Vector3(player.position.x, player.position.y - 10f, player.position.z), player.transform.localRotation) as GameObject;
            go.transform.parent = player.transform.parent;
        }
    }
}