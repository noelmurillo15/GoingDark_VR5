using UnityEngine;

public class Asteroid : MonoBehaviour {

    private float m_aliveTimer;
    private Vector3 m_velocity;
    private Vector3 m_rotation;

    // Use this for initialization
    void Start() {
        m_aliveTimer = Random.Range(120.0f, 360.0f);

        m_velocity.x = Random.Range(-2.0f, 2.0f);
        m_velocity.y = Random.Range(-2.0f, 2.0f);
        m_velocity.z = Random.Range(-2.0f, 2.0f);

        m_rotation.x = Random.Range(-15.0f, 15.0f);
        m_rotation.y = Random.Range(-15.0f, 15.0f);
        m_rotation.z = Random.Range(-15.0f, 15.0f);

        m_velocity = Vector3.zero;
        m_rotation = Vector3.zero;

        Vector3 m_scale = Vector3.zero;
        m_scale.x = Random.Range(5f, 50.0f);
        m_scale.y = m_scale.x;
        m_scale.z = m_scale.x;

        Vector3 newScale = transform.localScale;
        newScale.x *= m_scale.x;
        newScale.y *= m_scale.y;
        newScale.z *= m_scale.z;
        transform.localScale = newScale;
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
    }

    public void Kill() {
        AsteroidGenerator m_generator = GameObject.Find("Environment").GetComponent<AsteroidGenerator>();
        m_generator.DeleteAsteroid();
        Destroy(this.gameObject);
    }
}
