using UnityEngine;

public class ShootObject : MonoBehaviour {
    //**  Attach to Player prefab  **//
    public int MissileCount;
    public float fireCooldown;
    private GameObject Missile;
    private GameObject player;


    void Start()
    {
        fireCooldown = 0.0f;
        MissileCount = 10;
        Missile = Resources.Load<GameObject>("PlayerMissile");

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {
        if (fireCooldown > 0.0f)
            fireCooldown -= Time.deltaTime;

        if (Input.GetKey(KeyCode.F))
            FireMissile();
    }

    public float GetFireCooldown()
    {
        return fireCooldown;
    }

    public void FireMissile()
    {
        if (fireCooldown <= 0.0f)
        {
            
            if (MissileCount > 0 )
            {
                fireCooldown = 1.0f;
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

    void OnTriggerEnter(Collider col)
    {
        if(col.name == "rightPalm" && fireCooldown <= 0.0f)
        {
            FireMissile();
        }
    }
}
