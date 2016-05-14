using UnityEngine;
using GD.Core.Enums;

public class MissileSystem : MonoBehaviour
{

    #region Properties
    public MissileType Type { get; private set; }
    public float Cooldown { get; private set; }

    public int MissileCount;
    private GameObject Missile;
    #endregion


    void Start()
    {
        Type = MissileType.BASIC;
        Cooldown = 0.0f;
        MissileCount = 10;
        MissileSelect(Type);
    }

    void Update()
    {
        if (Cooldown > 0.0f)
            Cooldown -= Time.deltaTime;

        if (Input.GetKey(KeyCode.F))
            FireMissile();

        if (Input.GetKey(KeyCode.Keypad1))
            MissileSelect(MissileType.BASIC);
        if (Input.GetKey(KeyCode.Keypad2))
            MissileSelect(MissileType.EMP);
        if (Input.GetKey(KeyCode.Keypad3))
            MissileSelect(MissileType.CHROMATIC);
        if (Input.GetKey(KeyCode.Keypad4))
            MissileSelect(MissileType.SHIELDBREAKER);
    }

    public void FireMissile()
    {
        if (Cooldown <= 0.0f)
        {
            if (MissileCount > 0)
            {
                Cooldown = 1.0f;
                if (Missile != null)
                {
                    MissileCount--;
                    
                    GameObject go = Instantiate(Missile, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z - 1f), transform.rotation) as GameObject;
                    go.transform.parent = transform;
                }
                else
                    Debug.Log("No Missile Gameobj attached");
            }
        }
    }
    public void AddMissile()
    {
        MissileCount++;
        Debug.Log("Missile Added");
    }
    public int GetMissileCount()
    {
        return MissileCount;
    }    

    public void MissileSelect(MissileType _type)
    {       
        switch (Type)
        {
            case MissileType.BASIC:
                Missile = Resources.Load<GameObject>("Missiles/BasicMissile");
                break;

            case MissileType.EMP:
                Missile = Resources.Load<GameObject>("Missiles/EmpMissile");
                break;

            case MissileType.CHROMATIC:
                Missile = Resources.Load<GameObject>("Missiles/ChromaticMissile");
                break;

            case MissileType.SHIELDBREAKER:
                Missile = Resources.Load<GameObject>("Missiles/ShieldBreakMissile");
                break;

            default:
                Debug.Log("Invalid Missile Type : " + _type.ToString());
                break;
        }

        Debug.Log(_type.ToString() + " Missile Selected");
        Type = _type;
    }
}
