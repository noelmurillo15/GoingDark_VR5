using UnityEngine;
using GD.Core.Enums;

public class MissileSystem : ShipDevice
{

    #region Properties
    public MissileType Type { get; private set; }
    public int Count { get; private set; }

    private GameObject environment;
    private GameObject missilePrefab;
    #endregion


    void Start()
    {
        Count = 10;
        maxCooldown = 5f;
        Type = MissileType.BASIC;
        MissileSelect(Type);
        environment = GameObject.Find("Environment");
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
        if (missilePrefab == null)
        {
            Debug.Log("Missile Gameobject not attached");
            return;
        }
        if (Count > 0)
        {
            Count--;
            GameObject go = Instantiate(missilePrefab, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z - 1f), transform.rotation) as GameObject;
            go.transform.parent = environment.transform;
        }
    }

    public void AddMissile()
    {
        int rand = Random.Range(2, 5);
        Count += rand;
        Debug.Log(rand + " Missiles Added");
    }

    public void MissileSelect(MissileType _type)
    {       
        switch (Type)
        {
            case MissileType.BASIC:
                missilePrefab = Resources.Load<GameObject>("Missiles/BasicMissile");
                break;

            case MissileType.EMP:
                missilePrefab = Resources.Load<GameObject>("Missiles/EmpMissile");
                break;

            case MissileType.CHROMATIC:
                missilePrefab = Resources.Load<GameObject>("Missiles/ChromaticMissile");
                break;

            case MissileType.SHIELDBREAKER:
                missilePrefab = Resources.Load<GameObject>("Missiles/ShieldBreakMissile");
                break;

            default:
                Debug.Log("Invalid Missile Type : " + _type.ToString());
                break;
        }

        Debug.Log(_type.ToString() + " Missile Selected");
        Type = _type;
    }
}
