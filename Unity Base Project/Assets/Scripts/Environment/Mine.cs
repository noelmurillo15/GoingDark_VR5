using UnityEngine;

public class Mine : MonoBehaviour
{
    public bool MineArmed;
    private GameObject Explosion;

    // Use this for initialization
    void Start()
    {
        MineArmed = true;
        Explosion = transform.GetChild(1).gameObject;
        Explosion.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player") || col.CompareTag("Decoy") || col.CompareTag("Enemy") && MineArmed)
        {
            col.transform.SendMessage("SplashDmg");
            Trigger();
        }
    }

    void Trigger()
    {
        Explosion.SetActive(true);
        MineArmed = false;
        Invoke("Kill", 1f);
    }

    void Kill()
    {
        Destroy(gameObject);
    }
}