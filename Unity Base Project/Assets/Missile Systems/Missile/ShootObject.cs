using UnityEngine;
using System.Collections;

public class ShootObject : MonoBehaviour
{

    public GameObject TargetPos;
    public GameObject Miss;
    public int MissileLimit;
    public int MissileCount;
    public GameObject[] Missiles;
    public float fireCooldown;

    public GameObject killCam;

    void Start()
    {
        fireCooldown = 0.0f;
        killCam = GameObject.Find("KillCam");
        killCam.SetActive(false);
    }

    void Update()
    {
        if (fireCooldown > 0.0f)
            fireCooldown -= Time.deltaTime;
        else
            killCam.SetActive(false);

    }

    public float GetFireCooldown()
    {
        return fireCooldown;
    }

    public void FireMissile()
    {
        if (fireCooldown <= 0.0f)
        {
            killCam.SetActive(true);
            Missiles = GameObject.FindGameObjectsWithTag("Missile");
            
            if (MissileCount <= MissileLimit - 1)
            {
                fireCooldown = 10.0f;
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
}
