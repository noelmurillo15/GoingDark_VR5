using UnityEngine;
using GD.Core.Enums;
using UnityEngine.UI;

public class MissileSystem : ShipDevice
{

    #region Properties
    public int Count { get; private set; }

    //  Missile Types
    private GameObject basic;
    private GameObject emp;
    private GameObject chromatic;
    private GameObject shieldbreak;
    private GameObject selectedMissile;

    private Transform MyTransform;
    private Transform leapcam;

    private bool missSwitch;
    private float missTimer;
    private int Wchoice;
    //  Missile Display
    private Text textCount;
    private Text textMissileChoice;

    #endregion


    void Start()
    {
        missSwitch = false;
        missTimer = 0.5f;
        Count = 15;
        maxCooldown = 1f;
        Wchoice = 0;

        basic = Resources.Load<GameObject>("Missiles/BasicMissile");
        emp = Resources.Load<GameObject>("Missiles/EmpMissile");
        chromatic = Resources.Load<GameObject>("Missiles/ChromaticMissile");
        shieldbreak = Resources.Load<GameObject>("Missiles/ShieldBreakMissile");

        MyTransform = transform;
        leapcam = GameObject.FindGameObjectWithTag("MainCamera").transform;

        textCount = GameObject.Find("MissileCounter").GetComponent<Text>();
        textMissileChoice = GameObject.Find("MissileChoice").GetComponent<Text>();

        textMissileChoice.text = "Basic Missile";
        textCount.text = Count.ToString();

        selectedMissile = basic;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.F))
            Activate();

        if (Input.GetAxisRaw("RBumper") > 0f)
            Activate();

        if (Input.GetButtonDown("Y"))
            missSwitch = true;

        if (missSwitch /*&& missTimer <= 0*/ )
        {
            Wchoice++;
            SwitchWeapon();
            missSwitch = false;
        }

        if (Activated)
            LaunchMissile();

        MyTransform.rotation = leapcam.rotation;
    }

    public void SwitchWeapon()
    {
        if (Wchoice >= 5)
            Wchoice = 0;

        if (Wchoice == 0)
        {
            selectedMissile = basic;
            textMissileChoice.text = "Basic Selected"; //selectedMissile.ToString();
            Debug.Log("basic is current weapon");
        }
        else if (Wchoice == 1)
        {
            selectedMissile = emp;
            textMissileChoice.text = "EMP Selected";
            Debug.Log("emp is current weapon");
        }
        else if (Wchoice == 2)
        {
            selectedMissile = shieldbreak;
            textMissileChoice.text = "ShieldBreaker Selected"; //selectedMissile.ToString();
            Debug.Log("shieldbreak is current weapon");
        }
        else if (Wchoice == 3)
        {
            selectedMissile = chromatic;
            textMissileChoice.text = "Chromatic Selected";// selectedMissile.ToString();
            Debug.Log("chromatic is current weapon");
        }

    }

    public void LaunchMissile()
    {
        if (selectedMissile == null)
        {
            Debug.Log("Missile Gameobject not attached");
            return;
        }

        if (Count > 0)
        {
            Count--;
            textCount.text = Count.ToString();
            DeActivate();
            GameObject go = Instantiate(selectedMissile, new Vector3(transform.position.x, transform.position.y, transform.position.z + .5f), transform.rotation) as GameObject;
            AudioManager.instance.PlayMissileLaunch();
        }
    }

    public void AddMissile()
    {
        int rand = Random.Range(3, 6);
        Count += rand;
        textCount.text = Count.ToString();
    }

    public void WeaponSelect(MissileType type)
    {
        switch (type)
        {
            case MissileType.EMP:
                selectedMissile = emp;
                textMissileChoice.text = "EMP Selected";// selectedMissile.ToString();
                break;
            case MissileType.BASIC:
                selectedMissile = basic;
                textMissileChoice.text = "Basic Selected"; //selectedMissile.ToString();
                break;
            case MissileType.CHROMATIC:
                selectedMissile = chromatic;
                textMissileChoice.text = "Chromatic Selected";// selectedMissile.ToString();
                break;
            case MissileType.SHIELDBREAKER:
                selectedMissile = shieldbreak;
                textMissileChoice.text = "ShieldBreaker";// selectedMissile.ToString();
                break;
        }
    }
}
