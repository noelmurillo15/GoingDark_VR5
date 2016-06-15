using UnityEngine;
using GoingDark.Core.Enums;
using UnityEngine.UI;

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
        basic = Resources.Load<GameObject>("Projectiles/Missiles/BasicMissile");
        emp = Resources.Load<GameObject>("Projectiles/Missiles/EmpMissile");
        chrome = Resources.Load<GameObject>("Projectiles/Missiles/ChromaticMissile");
        shieldbreak = Resources.Load<GameObject>("Projectiles/Missiles/ShieldBreakMissile");        

        pool = new ObjectPooling();
        pool.Initialize(basic, 4);

        pool2 = new ObjectPooling();
        pool2.Initialize(emp, 4);

        pool3 = new ObjectPooling();
        pool3.Initialize(shieldbreak, 4);

        pool4 = new ObjectPooling();
        pool4.Initialize(chrome, 4);
    }

    void Update()
    {
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
            if (currentType == MissileType.Basic)
            {
                GameObject obj = pool.GetPooledObject();
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
                obj.SetActive(true);
                obj.SendMessage("SelfDestruct");
            }
            if (currentType == MissileType.Emp)
            {
                GameObject obj = pool2.GetPooledObject();
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
                obj.SetActive(true);
                obj.SendMessage("SelfDestruct");
            }
            if (currentType == MissileType.ShieldBreak)
            {
                GameObject obj = pool3.GetPooledObject();
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
                obj.SetActive(true);
                obj.SendMessage("SelfDestruct");
            }
            if (currentType == MissileType.Chromatic)
            {
                GameObject obj = pool4.GetPooledObject();
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
                obj.SetActive(true);
                obj.SendMessage("SelfDestruct");
            }
            AudioManager.instance.PlayMissileLaunch();
        }
        else
            Count = 15;
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
            case MissileType.Basic:
                textMissileChoice.text = "Basic Selected";
                break;
            case MissileType.Emp:
                textMissileChoice.text = "EMP Selected";
                break;
            case MissileType.ShieldBreak:
                textMissileChoice.text = "ShieldBreaker";
                break;
            case MissileType.Chromatic:
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
                case MissileType.Basic:
                    textMissileChoice.text = "Basic Selected";
                    break;
                case MissileType.Emp:
                    textMissileChoice.text = "EMP Selected";
                    break;
                case MissileType.ShieldBreak:
                    textMissileChoice.text = "ShieldBreaker";
                    break;
                case MissileType.Chromatic:
                    textMissileChoice.text = "Chromatic Selected";
                    break;
            }
        }
    }
}
