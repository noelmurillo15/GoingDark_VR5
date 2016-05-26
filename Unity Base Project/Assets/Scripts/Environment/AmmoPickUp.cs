using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{
    private MissileSystem missile;
    bool collected;
    // Use this for initialization
    void Start()
    {
        collected = false;
        missile = GameObject.Find("Devices").GetComponentInChildren<MissileSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (collected)
            Destroy(gameObject);
    }
   
    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player")
        {
            missile.SendMessage("AddMissile");
            collected = true;
        }
    }
}