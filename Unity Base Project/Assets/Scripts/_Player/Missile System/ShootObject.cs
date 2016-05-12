using UnityEngine;

public class ShootObject : MonoBehaviour {
    public float Cooldown { get; private set; }
    public int MissileCount;
    private GameObject Missile;
    private GameObject player;


    void Start()
    {
        Cooldown = 0.0f;
        MissileCount = 10;

        player = GameObject.FindGameObjectWithTag("Player");
        Missile = Resources.Load<GameObject>("PlayerMissile");
    }

    void Update() {
        if (Cooldown > 0.0f)
            Cooldown -= Time.deltaTime;

        if (Input.GetKey(KeyCode.F))
            FireMissile();
    }

    public void FireMissile()
    {
        if (Cooldown <= 0.0f)
        {
            
            if (MissileCount > 0 )
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

    void OnTriggerEnter(Collider col)
    {
        if(col.name == "rightPalm" && Cooldown <= 0.0f)
        {
            FireMissile();
        }
    }

    public int GetMissileCount()
    {
        return MissileCount;
    }
}
