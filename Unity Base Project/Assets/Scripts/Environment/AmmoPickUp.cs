using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{
    private MissileSystem missile;
    private Transform player;
    private Transform myTransform;

    bool collected;
    // Use this for initialization
    void Start()
    {
        myTransform = transform;
        collected = false;
        missile = GameObject.Find("Devices").GetComponentInChildren<MissileSystem>();
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (collected)
            Destroy(gameObject);

        if(!collected)
        {
            myTransform.LookAt(player);
            myTransform.position += myTransform.forward * 100 * Time.deltaTime;
        }
    }
   
    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player")
        {
            missile.SendMessage("AddMissile");
            AudioManager.instance.PlayAmmoPickUp();
            collected = true;
        }
    }
}