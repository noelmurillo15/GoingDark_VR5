using UnityEngine;

public class Asteroid : MonoBehaviour {

    public bool skipStart;

    private float m_aliveTimer;
    private Vector3 m_velocity;
    private Vector3 m_rotation;    

    // Use this for initialization
    void Start() {
        if (!skipStart)
        {
            m_aliveTimer = Random.Range(120.0f, 360.0f);

            m_velocity.x = Random.Range(-2.0f, 2.0f);
            m_velocity.y = Random.Range(-2.0f, 2.0f);
            m_velocity.z = Random.Range(-2.0f, 2.0f);

            m_rotation.x = Random.Range(-15.0f, 15.0f);
            m_rotation.y = Random.Range(-15.0f, 15.0f);
            m_rotation.z = Random.Range(-15.0f, 15.0f);

            Vector3 m_scale = Vector3.zero;
            m_scale.x = Random.Range(0.5f, 20.0f);
            m_scale.y = m_scale.x;
            m_scale.z = m_scale.x;

            Vector3 newScale = transform.localScale;
            newScale.x *= m_scale.x;
            newScale.y *= m_scale.y;
            newScale.z *= m_scale.z;
            transform.localScale = newScale;
        }
        else
        {
            m_aliveTimer = 12000.0f;
            m_velocity = Vector3.zero;

            m_rotation.x = Random.Range(1.0f, 360.0f);
            m_rotation.y = Random.Range(1.0f, 360.0f);
            m_rotation.z = Random.Range(1.0f, 360.0f);

            transform.Rotate(m_rotation);
            m_rotation = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update() {
        if (m_aliveTimer > 0.0)
            m_aliveTimer -= Time.deltaTime;
        else
            Kill();

        transform.Rotate(m_rotation * Time.deltaTime);
        transform.Translate(m_velocity * Time.deltaTime);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.GetType() == typeof(MeshCollider) && col.transform.CompareTag("Missile"))
        {
            col.transform.SendMessage("Kill");
            Kill();
        }

        if (col.transform.CompareTag("PlayerShip"))
        {
            col.transform.parent.SendMessage("Hit");
        }
    }

    public void Kill() {
        AsteroidGenerator m_generator = GameObject.Find("Environment").GetComponent<AsteroidGenerator>();
        m_generator.DeleteAsteroid();
        Destroy(this.gameObject);
    }
}
