using UnityEngine;

public class Asteroid : MonoBehaviour {

    private Vector3 m_velocity;
    private Vector3 m_rotation;
    private Transform MyTransform;

    // Use this for initialization
    void Start() {
        MyTransform = transform;
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

        Vector3 newScale = MyTransform.localScale;
        newScale.x *= m_scale.x;
        newScale.y *= m_scale.y;
        newScale.z *= m_scale.z;
        MyTransform.localScale = newScale;
    }

    // Update is called once per frame
    void FixedUpdate() {
        MyTransform.Rotate(m_rotation * Time.deltaTime);
        MyTransform.Translate(m_velocity * Time.deltaTime);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.GetType() == typeof(MeshCollider) && col.transform.CompareTag("Missile"))
        {
            col.transform.SendMessage("Kill");
            Kill();
        }
    }

    void OnBecameVisible()
    {
        enabled = true;
    }

    void OnBecameInvisible()
    {
        enabled = false;
    }

    public void Kill() {
        AsteroidGenerator m_generator = transform.parent.GetComponentInParent<AsteroidGenerator>();
        m_generator.DeleteAsteroid();
        Destroy(gameObject);
    }
}
