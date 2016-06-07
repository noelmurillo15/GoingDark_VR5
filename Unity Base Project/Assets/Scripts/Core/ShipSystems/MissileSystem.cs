using UnityEngine;
using GD.Core.Enums;
using UnityEngine.UI;
using System.Linq;


public class MissileSystem : ShipDevice
{

    #region Properties
    public int Count { get; private set; }

    private Transform MyTransform;
    private Transform leapcam;

    private float buffer;

    //  Missile Display
    private Text textCount;
    private Text textMissileChoice;

    //  Missile Data
    public MissileType currentType;

    public ObjectPooling pool;
    public ObjectPooling pool2;
    public ObjectPooling pool3;
    public ObjectPooling pool4;

    private GameObject basic;
    private GameObject emp;
    private GameObject chrome;
    private GameObject shieldbreak;

    private int choice;
    #endregion


    void Start()
    {
        buffer = 0f;
        Count = 15;
        maxCooldown = 1f;

        //  Leap Aim
        MyTransform = transform;
        leapcam = GameObject.FindGameObjectWithTag("MainCamera").transform;

        // Show selected missile text
        textMissileChoice = GameObject.Find("MissileChoice").GetComponent<Text>();
        textMissileChoice.text = "Basic Missile";

        // Show missile count
        textCount = GameObject.Find("MissileCounter").GetComponent<Text>();
        textCount.text = Count.ToString();

        // Missile Data
        basic = Resources.Load<GameObject>("Missiles/BasicMissile");
        emp = Resources.Load<GameObject>("Missiles/EmpMissile");
        chrome = Resources.Load<GameObject>("Missiles/ChromaticMissile");
        shieldbreak = Resources.Load<GameObject>("Missiles/ShieldBreakMissile");

        pool = new ObjectPooling();
        pool.Initialize(basic, 3);

        pool2 = new ObjectPooling();
        pool2.Initialize(emp, 3);

        pool3 = new ObjectPooling();
        pool3.Initialize(shieldbreak, 3);

        pool4 = new ObjectPooling();
        pool4.Initialize(chrome, 3);

        choice = 1;

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.F))
            Activate();

        if (Input.GetAxisRaw("RBumper") > 0f)
            Activate();

        if (Input.GetButtonDown("Y"))
            WeaponSwap();
    
        if (Activated)
            LaunchMissile();

        if (buffer > 0f)
            buffer -= Time.deltaTime;

        MyTransform.rotation = leapcam.rotation;
    }

    public void LaunchMissile()
    {
        if (Count > 0)
        {
            Count--;
            DeActivate();
            textCount.text = Count.ToString();
            if (choice == 1)
            {
                GameObject obj = pool.GetPooledObject();
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
                obj.SetActive(true);
                obj.SendMessage("SelfDestruct");
            }
            if (choice == 2)
            {
                GameObject obj = pool2.GetPooledObject();
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
                obj.SetActive(true);
                obj.SendMessage("SelfDestruct");
            }
            if (choice == 3)
            {
                GameObject obj = pool3.GetPooledObject();
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
                obj.SetActive(true);
                obj.SendMessage("SelfDestruct");
            }
            if (choice == 4)
            {
                GameObject obj = pool4.GetPooledObject();
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
                obj.SetActive(true);
                obj.SendMessage("SelfDestruct");
            }
            AudioManager.instance.PlayMissileLaunch();
        }
    }

    public void AddMissile()
    {
        int rand = Random.Range(1, 3);
        Count += rand;
        textCount.text = Count.ToString();
    }

    public void WeaponSelect(MissileType type)
    {
        currentType = type;
        switch (currentType)
        {
            case MissileType.BASIC:
                textMissileChoice.text = "Basic Selected";
                break;
            case MissileType.EMP:
                textMissileChoice.text = "EMP Selected";
                break;
            case MissileType.SHIELDBREAKER:
                textMissileChoice.text = "ShieldBreaker";
                break;
            case MissileType.CHROMATIC:
                textMissileChoice.text = "Chromatic Selected";
                break;
        }
    }

    public void WeaponSwap()
    {
        if (buffer <= 0f)
        {
            buffer = .5f;
            int curr = (int)(currentType + 1);
            if (System.Enum.GetValues(typeof(MissileType)).Length == curr)
                curr = 0;

            currentType = (MissileType)curr;
            switch (currentType)
            {
                case MissileType.BASIC:
                    textMissileChoice.text = "Basic Selected";
                    choice = 1;
                    break;
                case MissileType.EMP:
                    textMissileChoice.text = "EMP Selected";
                    choice = 2;
                    break;
                case MissileType.SHIELDBREAKER:
                    textMissileChoice.text = "ShieldBreaker";
                    choice = 3;
                    break;
                case MissileType.CHROMATIC:
                    textMissileChoice.text = "Chromatic Selected";
                    choice = 4;
                    break;
            }
        }
    }
}
