using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

    public Vector3 m_scale;
    public Vector3 m_velocity;
    public Vector3 m_position;
    public Vector3 m_rotation;
    public AsteroidGenerator m_generator;

    // Use this for initialization
    void Start() {

        m_velocity.x = Random.Range(-1.0f, 1.0f);
        m_velocity.y = Random.Range(-1.0f, 1.0f);
        m_velocity.z = Random.Range(-1.0f, 1.0f);

        m_position.x = Random.Range(-100.0f, 100.0f);
        m_position.y = Random.Range(-100.0f, 100.0f);
        m_position.z = Random.Range(-100.0f, 100.0f);

        m_scale.x = Random.Range(50.0f, 500.0f);
        m_scale.y = Random.Range(50.0f, 500.0f);
        m_scale.z = Random.Range(50.0f, 500.0f);

        m_rotation.x = Random.Range(-10.0f, 10.0f);
        m_rotation.y = Random.Range(-10.0f, 10.0f);
        m_rotation.z = Random.Range(-10.0f, 10.0f);

        if (m_generator == null)
            m_generator = GameObject.Find("Environment").GetComponent<AsteroidGenerator>();

        transform.localPosition = m_position;
        transform.localScale += m_scale;
    }

    // Update is called once per frame
    void Update() {

        if (CheckOutOfBounds())
            DestroyAsteroid();

        transform.Rotate(m_rotation * Time.deltaTime);
        transform.Translate(m_velocity * Time.deltaTime);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.name == "Player")
        {
            col.GetComponent<PlayerData>().Hit();
        }
    }

    bool CheckOutOfBounds() {
        if (transform.localPosition.x > 500.0f || transform.localPosition.x < -500.0f)
            return true;                                                       
        if (transform.localPosition.y > 500.0f || transform.localPosition.y < -500.0f)
            return true;
        if (transform.localPosition.z > 500.0f || transform.localPosition.z < -500.0f)
            return true;

        return false;
    }

    void DestroyAsteroid() {
        m_generator.DeleteAsteroid();
        Destroy(this.gameObject);
    }
}
