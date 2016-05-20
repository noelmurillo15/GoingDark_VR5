using UnityEngine;

public class Asteroid : MonoBehaviour
{
    #region Properties
    public bool skipStart;
    private Vector3 m_velocity;
    private Vector3 m_rotation;
    private Transform MyTransform;
    #endregion


    void Start()
    {
        MyTransform = transform;
        if (!skipStart)
        {
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

            Vector3 newScale = MyTransform.localScale;
            newScale.x *= m_scale.x;
            newScale.y *= m_scale.y;
            newScale.z *= m_scale.z;
            MyTransform.localScale = newScale;
        }
        else
        {
            Invoke("SelfDestruct", 10f);
            m_velocity.x = Random.Range(-35.0f, 35.0f);
            m_velocity.y = Random.Range(-35.0f, 35.0f);
            m_velocity.z = Random.Range(-35.0f, 35.0f);

            m_rotation.x = Random.Range(1.0f, 360.0f);
            m_rotation.y = Random.Range(1.0f, 360.0f);
            m_rotation.z = Random.Range(1.0f, 360.0f);
            MyTransform.localEulerAngles = m_rotation;
            m_rotation = Vector3.zero;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        MyTransform.Rotate(m_rotation * Time.deltaTime);
        MyTransform.Translate(m_velocity * Time.deltaTime);
    }
    private bool RandomChance()
    {
        int randValue = Random.Range(0, 100);
        if (randValue > 0)
        {
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
            float range = Random.Range(2, 16);

            for (int i = 0; i < range; i++)
            {
                SendMessage("AutoScale", MyTransform.localScale);
                GameObject go = Instantiate(gameObject, new Vector3(MyTransform.position.x,
                MyTransform.position.y, MyTransform.position.z), MyTransform.rotation) as GameObject;
                go.transform.parent = MyTransform.parent;
            }
        }
        Destroy(gameObject);
    }
    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
    public void AutoScale(Vector3 _scale)
    {
        _scale *= 0.5f;
        MyTransform.localScale = _scale;
    }
}