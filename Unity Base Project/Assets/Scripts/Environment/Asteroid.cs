using UnityEngine;

public class Asteroid : MonoBehaviour {

    public bool skipStart;

    private float m_aliveTimer;
    private Vector3 m_velocity;
    private Vector3 m_rotation;
    private float asteroidTimer;

    // Use this for initialization
    void Start()
    {
        asteroidTimer = 7.0f;

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
            m_aliveTimer = Random.Range(120.0f, 360.0f);

            m_velocity.x = Random.Range(-35.0f, 35.0f);
            m_velocity.y = Random.Range(-35.0f, 35.0f);
            m_velocity.z = Random.Range(-35.0f, 35.0f);

            m_rotation.x = Random.Range(1.0f, 360.0f);
            m_rotation.y = Random.Range(1.0f, 360.0f);
            m_rotation.z = Random.Range(1.0f, 360.0f);
            this.transform.localEulerAngles = m_rotation;
            m_rotation = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_aliveTimer > 0.0)
            m_aliveTimer -= Time.deltaTime;
        else
        {
            AsteroidGenerator m_generator = GameObject.Find("Environment").GetComponent<AsteroidGenerator>();
            m_generator.DeleteAsteroid();
            Destroy(this.gameObject);
        }

        transform.Rotate(m_rotation * Time.deltaTime);
        transform.Translate(m_velocity * Time.deltaTime);

        if (skipStart)
        {
            if (asteroidTimer > 0)
                asteroidTimer -= Time.deltaTime;

            if (asteroidTimer < 7.0f && asteroidTimer > 0.0f)
                transform.Translate(m_velocity * (asteroidTimer * 0.5f) * Time.deltaTime);
            else if (asteroidTimer <= 0.0f)
                m_velocity -= (m_velocity.normalized * Time.deltaTime * 50.0f);
        }
    }
    private bool RandomChance()
    {
        int randValue = Random.Range(0, 100);
        if (randValue > 0)
        {
            Debug.Log("Chance Pass");
            return true;
        }
        return false;
    }

    void OnBecameVisible()
    {
        enabled = true;
    }

    void OnBecameInvisible()
    {
        enabled = false;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.GetType() == typeof(MeshCollider) && col.transform.CompareTag("Missile"))
        {
            col.transform.SendMessage("Kill");
            Kill();
        }
    }

    public void Kill()
    {
        AsteroidGenerator m_generator = GameObject.Find("Environment").GetComponent<AsteroidGenerator>();
        m_generator.DeleteAsteroid();

        if (RandomChance())
        {
            skipStart = true;
            float range = Random.Range(2, 4);

            for (int i = 0; i < range; i++)
            {
                SendMessage("AutoScale", this.transform.localScale);
                Instantiate(this.gameObject, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
            }
        }
        Destroy(this.gameObject);
    }
    public void AutoScale(Vector3 _scale)
    {
        _scale /= 2.0f;
        this.transform.localScale = _scale;
        asteroidTimer = 7.0f;
    }
}
