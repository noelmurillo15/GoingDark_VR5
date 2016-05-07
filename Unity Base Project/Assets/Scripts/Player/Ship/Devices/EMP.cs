using UnityEngine;
using System.Collections;

public class EMP : MonoBehaviour {
    //**    Attach to EMP Sphere    **/
    public bool isEmpActive;
    public float EmpCooldown;
    private Transform m_transform;


    // Use this for initialization
    void Start() {
        isEmpActive = false;
        EmpCooldown = 0.0f;

        if (m_transform == null)
            m_transform = GameObject.Find("EMP").transform;
    }

    // Update is called once per frame
    void Update() {
        if (EmpCooldown > 0.0f)
            EmpCooldown -= Time.deltaTime;

        if (isEmpActive) {
            m_transform.localScale = new Vector3(
                m_transform.localScale.x + (50 * Time.deltaTime),
                m_transform.localScale.y + (50 * Time.deltaTime),
                m_transform.localScale.z + (50 * Time.deltaTime));

            if (m_transform.localScale.x >= 100.0f) {
                m_transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                SetEmpActive(false);
                EmpCooldown = 30.0f;
            }
        }
    }
    public bool GetEmpActive() {
        return isEmpActive;
    }

    public float GetEmpCooldown() {
        return EmpCooldown;
    }

    public void SetEmpActive(bool flip) {
        isEmpActive = flip;
    }
}