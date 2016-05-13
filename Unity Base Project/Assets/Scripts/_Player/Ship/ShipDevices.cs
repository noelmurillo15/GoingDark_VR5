using UnityEngine;

public class ShipDevices : MonoBehaviour
{

    #region Properties
    public EMP Emp { get; private set; }
    public Cloak Cloak { get; private set; }
    public Decoy Decoy { get; private set; }
    public HyperDrive HyperDrive { get; private set; }
    public ShootObject MissileLaunch { get; private set; }

    public GameObject Radar { get; private set; }
    public GameObject MissionLog { get; private set; }

    public GameObject WeaponSelect { get; private set; }
    #endregion


    // Use this for initialization
    void Start()
    {
        Radar = GameObject.Find("Radar");
        MissionLog = GameObject.Find("ButtonObject");
        Emp = GameObject.Find("EMP").GetComponent<EMP>();
        WeaponSelect = GameObject.Find("WeaponButtonObj");
        Decoy = GameObject.Find("Decoy").GetComponent<Decoy>();
        Cloak = GameObject.Find("Cloak").GetComponent<Cloak>();
        HyperDrive = GameObject.Find("HyperDrive").GetComponent<HyperDrive>();
        MissileLaunch = GameObject.Find("MissileLaunch").GetComponent<ShootObject>();

    }

    // Update is called once per frame
    void Update()
    {

    }
}