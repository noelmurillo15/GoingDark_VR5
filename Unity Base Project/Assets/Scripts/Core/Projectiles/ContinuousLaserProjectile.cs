using UnityEngine;

public class ContinuousLaserProjectile : MonoBehaviour
{
    public float lifetime = 8.0f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0.0f)
            Destroy(gameObject);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag("Enemy"))
        {
            col.gameObject.SendMessage("Kill");
        }
    }
}
