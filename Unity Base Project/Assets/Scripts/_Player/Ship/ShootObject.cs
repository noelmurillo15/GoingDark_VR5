using UnityEngine;
using GD.Core.Enums;

public class ShootObject : MonoBehaviour
{
    //**  Attach to Player prefab  **//
    public int MissileCount;
    public float Cooldown { get; private set; }
    private GameObject Missile;
    private GameObject player;
    public MissileType Type { get; private set; }

    void Start()
    {
        Type = MissileType.BASIC;
        Cooldown = 0.0f;
        MissileCount = 10;
        player = GameObject.FindGameObjectWithTag("Player");
        WeaponChoice(Type);
    }

    void Update()
    {

        if (Cooldown > 0.0f)
            Cooldown -= Time.deltaTime;

        if (Input.GetKey(KeyCode.F))
            FireMissile();

    }
    public MissileType GetMissileType()
    {
        return Type;
    }

    public void SetMissileType(MissileType _val)
    {
        Type = _val;
    }

    public void FireMissile()
    {
        WeaponChoice(Type);
        SetMissileType(Type);
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

    public MissileType WeaponChoice(MissileType _type)
    {
        Type = _type;

        switch (Type)
        {
            case MissileType.BASIC:
                Missile = Resources.Load<GameObject>("PlayerMissile");
                Debug.Log("Fired Regular Missile");
                return Type;
            case MissileType.EMP:
                Missile = Resources.Load<GameObject>("EMPMissile");
                Debug.Log("Fired EMP Missile");
                return Type;
            case MissileType.CHROMATIC:
                Missile = Resources.Load<GameObject>("ChromaticMissile");
                Debug.Log("Got ChromaticMissile");
                return Type;
            case MissileType.SHIELDBREAKER:
                Missile = Resources.Load<GameObject>("PlayerMissile");
                Debug.Log("Got ShieldBreaker");
                return Type;
        }

        return Type;
    }
}
