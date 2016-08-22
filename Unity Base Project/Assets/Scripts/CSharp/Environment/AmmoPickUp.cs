using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{
    private MissileSystem missile;
    private Transform player;
    private Transform myTransform;

    // Use this for initialization
    void Start()
    {
        myTransform = transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        missile = GameObject.Find("Devices").GetComponentInChildren<MissileSystem>();
    }

    void FixedUpdate()
    {
        myTransform.LookAt(player);
        myTransform.position += myTransform.forward * 200 * Time.fixedDeltaTime;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player")
        {
            missile.SendMessage("AddMissile");
            AudioManager.instance.PlayAmmoPickUp();
            GetComponent<Despawn>().Kill();
        }
    }
}