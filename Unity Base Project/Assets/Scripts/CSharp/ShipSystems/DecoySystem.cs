using UnityEngine;

public class DecoySystem : ShipSystem {

    #region Properties
    public int Count { get; private set; }

    private Transform player;
    public GameObject decoy;
    private GameObject GM;
    #endregion

    // Use this for initialization
    void Start()
    {
        Count = 5;
        maxCooldown = 15f;
        player = GameObject.FindGameObjectWithTag("MainCamera").transform;
        decoy = Resources.Load<GameObject>("Devices/DecoyShip");
        GM = GameObject.FindGameObjectWithTag("GameManager");
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
            GameObject go = Instantiate(decoy, new Vector3(player.position.x, player.position.y - 10f, player.position.z), player.transform.rotation) as GameObject;
            go.transform.parent = GM.transform;
        }
    }
}