using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

    public float aliveTimer;
    public Vector3 m_scale;
    public Vector3 m_velocity;
    public Vector3 m_position;
    public Vector3 m_rotation;
    public AsteroidGenerator m_generator;
    public GameObject m_player;

    // Use this for initialization
    void Start() {

        if(m_player == null)
            m_player = GameObject.FindGameObjectWithTag("Player");

        m_velocity.x = Random.Range(-15.0f, 15.0f);
        m_velocity.y = Random.Range(-15.0f, 15.0f);
        m_velocity.z = Random.Range(-8.0f, 8.0f);

        m_position.x = Random.Range((m_player.transform.localPosition.x - 10.0f) - 200.0f, (m_player.transform.localPosition.x + 10.0f) + 200.0f);
        m_position.y = Random.Range((m_player.transform.localPosition.y - 10.0f) - 200.0f, (m_player.transform.localPosition.y + 10.0f) + 200.0f);
        m_position.z = Random.Range((m_player.transform.localPosition.z - 10.0f) - 50.0f,  (m_player.transform.localPosition.z + 10.0f) + 50.0f);

        m_scale.x = Random.Range(50.0f, 200.0f);
        m_scale.y = Random.Range(50.0f, 200.0f);
        m_scale.z = Random.Range(50.0f, 200.0f);

        m_rotation.x = Random.Range(-5.0f, 5.0f);
        m_rotation.y = Random.Range(-5.0f, 5.0f);
        m_rotation.z = Random.Range(-5.0f, 5.0f);

        aliveTimer = Random.Range(30.0f, 120.0f);

        if (m_generator == null)
            m_generator = GameObject.Find("AsteroidGen").GetComponent<AsteroidGenerator>();

        transform.localPosition = m_position;
        transform.localScale += m_scale;
    }

    // Update is called once per frame
    void Update() {
        if (aliveTimer > 0)
            aliveTimer -= Time.deltaTime;
        else DestroyAsteroid();

        if (CheckOutOfBounds())
            DestroyAsteroid();

        transform.Rotate(m_rotation * Time.deltaTime);
        transform.Translate(m_velocity * Time.deltaTime);
    }

    bool CheckOutOfBounds() {
        if (transform.localPosition.x > (m_player.transform.localPosition.x + 250.0f) || transform.localPosition.x < (m_player.transform.localPosition.x - 250.0f))
            return true;                                                       
        if (transform.localPosition.y > (m_player.transform.localPosition.y + 250.0f) || transform.localPosition.y < (m_player.transform.localPosition.x - 250.0f))
            return true;
        if (transform.localPosition.z > (m_player.transform.localPosition.x + 150.0f) || transform.localPosition.z < (m_player.transform.localPosition.x - 150.0f))
            return true;

        return false;
    }
    void DestroyAsteroid() {
        m_generator.DeleteAsteroid();
        Destroy(this.gameObject);
    }
}
