using UnityEngine;

public class ShootObject : MonoBehaviour
{
    //**  Attach to Player prefab  **//
    public int MissileCount;
    public float Cooldown { get; private set; }
    private GameObject Missile;
    private GameObject player;
    public int MissileType;

    void Start()
    {
        MissileType = 2;
        Cooldown = 0.0f;
        MissileCount = 10;
        player = GameObject.FindGameObjectWithTag("Player");
        WeaponChoice(MissileType);
    }

    void Update()
    {

        if (Cooldown > 0.0f)
            Cooldown -= Time.deltaTime;

        if (Input.GetKey(KeyCode.F))
            FireMissile();

    }
    public int GetMissileType()
    {
        return MissileType;
    }

    public void SetMissileType(int _val)
    {
        MissileType = _val;
    }

    public void FireMissile()
    {
        WeaponChoice(MissileType);
        SetMissileType(MissileType);
        if (Cooldown <= 0.0f)
        {
            if (MissileCount > 0)
            {
                Cooldown = 1.0f;
                if (Missile != null)
                {
                    MissileCount--;
                    Instantiate(Missile, new Vector3(player.transform.localPosition.x, player.transform.localPosition.y - 15f, player.transform.localPosition.z + 10f), player.transform.localRotation);
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

    void OnTriggerEnter(Collider col)
    {
        if (col.name == "rightPalm" && Cooldown <= 0.0f)
            FireMissile();
    }

    public int WeaponChoice(int _type)
    {
        MissileType = _type;

        switch (MissileType)
        {
            case 0:
                Missile = Resources.Load<GameObject>("PlayerMissile");
                Debug.Log("Fired Regular Missile");
                return MissileType;
            case 1:
                Missile = Resources.Load<GameObject>("EMPMissile");
                Debug.Log("Fired EMP Missile");
                return MissileType;
            case 2:
                Missile = Resources.Load<GameObject>("ChromaticMissile");
                Debug.Log("Got ChromaticMissile");
                return MissileType;
            case 3:
                Missile = Resources.Load<GameObject>("PlayerMissile");
                Debug.Log("Got ShieldBreaker");
                return MissileType;
        }

        return MissileType;
    }
}
