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

    //  Missile Display
    private Text textCount;
    #endregion


    void Start()
    {
        missSwitch = false;
        missTimer = 5;
        Count = 15;
        maxCooldown = 1f;

        
        basic = Resources.Load<GameObject>("Missiles/BasicMissile");
        emp = Resources.Load<GameObject>("Missiles/EmpMissile");
        chromatic = Resources.Load<GameObject>("Missiles/ChromaticMissile");
        shieldbreak = Resources.Load<GameObject>("Missiles/ShieldBreakMissile");

        MyTransform = transform;
        leapcam = GameObject.FindGameObjectWithTag("MainCamera").transform;

        textCount = GameObject.Find("MissileCounter").GetComponent<Text>();
        textCount.text = Count.ToString();
        selectedMissile = emp;
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.F))
            Activate();

        if (Input.GetAxisRaw("LBumper") > 0f)
            Activate();

        if (Activated)
            LaunchMissile();

        MyTransform.rotation = leapcam.rotation;
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
                break;
            case MissileType.BASIC:
                selectedMissile = basic;
                break;
            case MissileType.CHROMATIC:
                selectedMissile = chromatic;
                break;
            case MissileType.SHIELDBREAKER:
                selectedMissile = shieldbreak;
                break;
        }
    }  
}
