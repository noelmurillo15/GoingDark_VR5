﻿using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {
    private Vector3 m_velocity;
    private Vector3 m_rotation;

    private Vector3 m_scale;
    private Vector3 m_position;
    private AsteroidGenerator m_generator;

    // Use this for initialization
    void Start() {

        m_velocity.x = Random.Range(-1.0f, 1.0f);
        m_velocity.y = Random.Range(-1.0f, 1.0f);
        m_velocity.z = Random.Range(-1.0f, 1.0f);

        m_position.x = Random.Range(-500.0f, 500.0f);
        m_position.y = Random.Range(-500.0f, 500.0f);
        m_position.z = Random.Range(-500.0f, 500.0f);

        m_scale.x = Random.Range(1.0f, 5.0f);
        m_scale.y = Random.Range(1.0f, 5.0f);
        m_scale.z = Random.Range(1.0f, 5.0f);

        m_rotation.x = Random.Range(-10.0f, 10.0f);
        m_rotation.y = Random.Range(-10.0f, 10.0f);
        m_rotation.z = Random.Range(-10.0f, 10.0f);

        if (m_generator == null)
            m_generator = GameObject.Find("Environment").GetComponent<AsteroidGenerator>();

        transform.localPosition = m_position;
        Vector3 newScale = transform.localScale;
        newScale.x *= m_scale.x;
        newScale.y *= m_scale.y;
        newScale.z *= m_scale.z;
        transform.localScale = newScale;
    }

    // Update is called once per frame
    void Update() {
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


    public void DestroyAsteroid() {
        m_generator.DeleteAsteroid();
        Destroy(this.gameObject);
    }
}
