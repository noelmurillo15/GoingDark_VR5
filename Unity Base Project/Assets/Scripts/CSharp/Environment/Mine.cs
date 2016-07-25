using UnityEngine;

public class Mine : MonoBehaviour
{
    private Transform MyTransform;
    public bool MineArmed;
    private GameObject Explosion;
    private Transform player;

    // Use this for initialization
    void Start()
    {
        MyTransform = transform;
        MineArmed = true;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Explosion = transform.GetChild(1).gameObject;
        Explosion.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") || col.CompareTag("Decoy") || col.CompareTag("Enemy") && MineArmed)
        {
            col.transform.SendMessage("Hit");
            Trigger();
        }
    }

    void Trigger()
    {
        Explosion.SetActive(true);
        Invoke("Kill", 1f);
        MineArmed = false;
    }

    void Kill()
    {
        Destroy(gameObject);
    }
}