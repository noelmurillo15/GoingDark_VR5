using UnityEngine;

public class Asteroid : MonoBehaviour
{

    #region Properties
    public bool skipStart;
    private Vector3 m_velocity;
    private Vector3 m_rotation;
    private BoxCollider boxcol;
    private Rigidbody MyRigidbody;
    private Transform MyTransform;
    private TallyScreen tallyscreen;
    #endregion


    void Start()
    {
        if(GameObject.FindGameObjectWithTag("GameManager") != null)
            tallyscreen = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TallyScreen>();

        MyTransform = transform;
        boxcol = GetComponent<BoxCollider>();        
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
            Invoke("EnableCollision", .25f);
            Invoke("SelfDestruct", 10f);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler(m_rotation * Time.fixedDeltaTime);
        MyRigidbody.MoveRotation(MyRigidbody.rotation * deltaRotation);
        MyRigidbody.MovePosition(MyRigidbody.position + (m_velocity * Time.fixedDeltaTime));
    }

    void OnBecameVisible()
    {        
        enabled = true;
    }

    void OnBecameInvisible()
    {
        enabled = false;
    }

    void EnableCollision()
    {
        boxcol.enabled = true;
    }
    void SelfDestruct()
    {
        Destroy(gameObject);
    }

    public void Kill()
    {
        skipStart = true;
        boxcol.enabled = false;
        AudioManager.instance.PlayAstDestroy();
        float range = Random.Range(3, 6);
        for (int i = 0; i < range; i++)
        {
            GameObject go = Instantiate(gameObject) as GameObject;

            if (go.transform.parent != null)
                go.transform.parent = MyTransform.parent;
            go.transform.localScale *= Random.Range(.1f, .45f);
        }

        if(tallyscreen != null)
            tallyscreen.AsteroidsDestroyed += 1;

        SelfDestruct();
    }    
}