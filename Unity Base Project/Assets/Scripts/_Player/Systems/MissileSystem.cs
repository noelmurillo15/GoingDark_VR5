using UnityEngine;
using GD.Core.Enums;

public class MissileSystem : MonoBehaviour
{

    #region Properties
    public MissileType Type { get; private set; }
    public float Cooldown { get; private set; }
    public int Count { get; private set; }

    private GameObject missilePrefab;
    private SystemsManager manager;
    #endregion


    void Start()
    {
        Type = MissileType.BASIC;
        Cooldown = 0.0f;
        Count = 10;
        MissileSelect(Type);
        manager = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemsManager>();
    }

    void Update()
    {
        if (Cooldown > 0.0f)
            Cooldown -= Time.deltaTime;

        if (Input.GetKey(KeyCode.F))
            manager.ActivateSystem(SystemType.MISSILES);

        if (Input.GetKey(KeyCode.Keypad1))
            MissileSelect(MissileType.BASIC);
        if (Input.GetKey(KeyCode.Keypad2))
            MissileSelect(MissileType.EMP);
        if (Input.GetKey(KeyCode.Keypad3))
            MissileSelect(MissileType.CHROMATIC);
        if (Input.GetKey(KeyCode.Keypad4))
            MissileSelect(MissileType.SHIELDBREAKER);
    }

    public void Activate()
    {
        if (Cooldown <= 0.0f)
        {
            if (Count > 0)
            {
                if (missilePrefab != null)
                {
                    Count--;                    
                    Cooldown = 5.0f;
                    GameObject go = Instantiate(missilePrefab, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z - 1f), transform.rotation) as GameObject;
                    go.transform.parent = transform;
                }
                else
                    Debug.Log("No Missile Gameobj attached");
            }
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
