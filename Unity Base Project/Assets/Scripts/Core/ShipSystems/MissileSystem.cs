using UnityEngine;
using GD.Core.Enums;

public class MissileSystem : ShipDevice
{

    #region Properties
    public MissileType Type { get; private set; }
    public int Count { get; private set; }

    //  Missle's parent
    private GameObject environment;

    //  Missile Types
    private GameObject basic;
    private GameObject emp;
    private GameObject chromatic;
    private GameObject shieldbreak;

    private GameObject selectedMissile;
    #endregion


    void Start()
    {
        Count = 10;
        maxCooldown = 5f;

        environment = GameObject.Find("Environment");

        basic = Resources.Load<GameObject>("Missiles/BasicMissile");
        emp = Resources.Load<GameObject>("Missiles/EmpMissile");
        chromatic = Resources.Load<GameObject>("Missiles/ChromaticMissile");
        shieldbreak = Resources.Load<GameObject>("Missiles/ShieldBreakMissile");

        Type = MissileType.BASIC;
        selectedMissile = basic;
    }

    void Update()
    {
        if (Cooldown == maxCooldown)
        {
            Debug.Log("Missile has been launched");
            LaunchMissile();
        }

        UpdateCooldown();
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
            GameObject go = Instantiate(selectedMissile, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z - 1f), transform.rotation) as GameObject;
            go.transform.parent = environment.transform;
        }
    }

    public void AddMissile()
    {
        int rand = Random.Range(2, 5);
        Count += rand;
        Debug.Log(rand + " Missiles Added");
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
