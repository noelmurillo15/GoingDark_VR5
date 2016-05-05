using UnityEngine;
using System.Collections;

public class ShootObject : MonoBehaviour
{
    //**  Attach to Player prefab  **//
    private GameObject Miss;
    public int MissileLimit;
    private int MissileCount;
    private GameObject[] Missiles;
    public float fireCooldown;

    void Start()
    {
        fireCooldown = 0.0f;
        MissileLimit = 5;
        Miss = Resources.Load<GameObject>("PlayerMissile");
    }

    void Update() {
        if (fireCooldown > 0.0f)
            fireCooldown -= Time.deltaTime;
    }

    public float GetFireCooldown()
    {
        return fireCooldown;
    }

    public void FireMissile()
    {
        if (fireCooldown <= 0.0f)
        {
            Missiles = GameObject.FindGameObjectsWithTag("Missile");
            
            if (MissileCount < MissileLimit )
            {
                fireCooldown = 2.0f;
                if (Miss != null)
                {
                    MissileCount++;
                    Instantiate(Miss, new Vector3(transform.localPosition.x, transform.localPosition.y - 10.0f, transform.localPosition.z + 10.0f), transform.rotation);
                }
                else
                    Debug.Log("No Missile Gameobj attached");
            }
        }
    }
    public void AddMissile()
    {
        MissileLimit++;
        Debug.Log("Missile Added");
    }
}
