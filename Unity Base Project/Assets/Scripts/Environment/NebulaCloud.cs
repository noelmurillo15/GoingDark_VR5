using UnityEngine;
using System.Collections;

public class NebulaCloud : MonoBehaviour {


    // Use this for initialization
    void Start() {

        Vector3 m_position = Vector3.zero;
        m_position.x = Random.Range(-750.0f, 750.0f);
        m_position.y = Random.Range(-300.0f, 300.0f);
        m_position.z = Random.Range(-750.0f, 750.0f);

        Vector3 m_scale = Vector3.zero;
        m_scale.x = Random.Range(1.0f, 12.0f);
        m_scale.y = m_scale.x;
        m_scale.z = m_scale.x;

        transform.localPosition = m_position;
        Vector3 newScale = transform.localScale;
        newScale.x *= m_scale.x;
        newScale.y *= m_scale.y;
        newScale.z *= m_scale.z;
        transform.localScale = newScale;
    }

    // Update is called once per frame
    void Update()
    {

    }
}