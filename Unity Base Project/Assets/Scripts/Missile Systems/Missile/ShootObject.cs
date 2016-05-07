using UnityEngine;

public class ShootObject : MonoBehaviour {
    //**  Attach to Player prefab  **//
    public int MissileCount;
    public float fireCooldown;
    private GameObject Miss;
    private GameObject[] Missiles;
    private GameObject player;


    void Start()
    {
        fireCooldown = 0.0f;
        MissileCount = 10;
        Miss = Resources.Load<GameObject>("PlayerMissile");

        player = GameObject.FindGameObjectWithTag("Player");
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
            
            if (MissileCount > 0 )
            {
                fireCooldown = 1.0f;
                if (Miss != null)
                {
                    MissileCount--;
                    Instantiate(Miss, new Vector3(player.transform.position.x, player.transform.position.y - 10.0f, player.transform.position.z), player.transform.rotation);
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

    void OnTriggerEnter(Collider col)
    {
//        Debug.Log(col.name + " has hit Fire Button");
        if(col.name == "rightPalm" && fireCooldown <= 0.0f)
        {
            FireMissile();
        }
    }
}
