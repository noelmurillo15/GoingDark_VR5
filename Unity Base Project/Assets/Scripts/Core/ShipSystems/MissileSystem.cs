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

    private GameObject basic;
    private GameObject emp;
    private GameObject chrome;
    private GameObject shieldbreak;

    private GameObject selectedMissile;
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
        textMissileChoice.text = "Chromatic Missile";

        // Show missile count
        textCount = GameObject.Find("MissileCounter").GetComponent<Text>();
        textCount.text = Count.ToString();

        // Missile Data
        basic = Resources.Load<GameObject>("Missiles/BasicMissile");
        emp = Resources.Load<GameObject>("Missiles/EmpMissile");
        chrome = Resources.Load<GameObject>("Missiles/ChromaticMissile");
        shieldbreak = Resources.Load<GameObject>("Missiles/ShieldBreakMissile");


        selectedMissile = chrome;
        currentType = MissileType.CHROMATIC;       
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
            GameObject go = Instantiate(selectedMissile, new Vector3(transform.position.x, transform.position.y, transform.position.z + .5f), transform.rotation) as GameObject;
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
                selectedMissile = basic;
                textMissileChoice.text = "Basic Selected";
                break;
            case MissileType.EMP:
                selectedMissile = emp;
                textMissileChoice.text = "EMP Selected";
                break;
            case MissileType.SHIELDBREAKER:
                selectedMissile = shieldbreak;
                textMissileChoice.text = "ShieldBreaker";
                break;
            case MissileType.CHROMATIC:
                selectedMissile = chrome;
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
                    selectedMissile = basic;
                    textMissileChoice.text = "Basic Selected";
                    break;
                case MissileType.EMP:
                    selectedMissile = emp;
                    textMissileChoice.text = "EMP Selected";
                    break;
                case MissileType.SHIELDBREAKER:
                    selectedMissile = shieldbreak;
                    textMissileChoice.text = "ShieldBreaker";
                    break;
                case MissileType.CHROMATIC:
                    selectedMissile = chrome;
                    textMissileChoice.text = "Chromatic Selected";
                    break;
            }
        }
    }
}
