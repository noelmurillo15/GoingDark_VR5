using UnityEngine;

public class Asteroid : MonoBehaviour
{

    #region Properties
    public bool skipStart;
    private Vector3 m_velocity;
    private Vector3 m_rotation;
    private Rigidbody MyRigidbody;
    private Transform MyTransform;
    #endregion


    void Start()
    {
        MyTransform = transform;
        MyRigidbody = GetComponent<Rigidbody>();
        if (!skipStart)
        {
            m_velocity.x = Random.Range(-30.0f, 30.0f);
            m_velocity.y = Random.Range(-30.0f, 30.0f);
            m_velocity.z = Random.Range(-30.0f, 30.0f);

            m_rotation.x = Random.Range(-15.0f, 15.0f);
            m_rotation.y = Random.Range(-15.0f, 15.0f);
            m_rotation.z = Random.Range(-15.0f, 15.0f);

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
        else
        {
            m_velocity.x = Random.Range(-150.0f, 150.0f);
            m_velocity.y = Random.Range(-150.0f, 150.0f);
            m_velocity.z = Random.Range(-150.0f, 150.0f);

            m_rotation.x = Random.Range(1.0f, 359.0f);
            m_rotation.y = Random.Range(1.0f, 359.0f);
            m_rotation.z = Random.Range(1.0f, 359.0f);
            MyTransform.localEulerAngles = m_rotation;

            m_rotation.x = Random.Range(-45.0f, 45.0f);
            m_rotation.y = Random.Range(-45.0f, 45.0f);
            m_rotation.z = Random.Range(-45.0f, 45.0f);
            Invoke("SelfDestruct", 30f);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler(m_rotation * Time.deltaTime);
        MyRigidbody.MoveRotation(MyRigidbody.rotation * deltaRotation);
        MyRigidbody.MovePosition(MyRigidbody.position + (m_velocity * Time.deltaTime));
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

    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
    public void Kill()
    {
        skipStart = true;
        AudioManager.instance.PlayHit();
        float range = Random.Range(3, 6);
        for (int i = 0; i < range; i++)
        {
            GameObject go = Instantiate(gameObject) as GameObject;
            go.transform.parent = MyTransform.parent;
            go.transform.localScale *= Random.Range(.1f, .45f);
        }
        Destroy(gameObject);
    }    
}