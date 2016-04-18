using UnityEngine;
using System.Collections;

public class PlayerKeyboardInput : MonoBehaviour {
    //**    Attach Script to Player Gameobject   **//

    private EMP m_emp;
    private PlayerData m_playerData;

    private float padding;


    // Use this for initialization
    void Start () {
        padding = 0.0f;

        if (m_playerData == null)
            m_playerData = GetComponent<PlayerData>();

        if (m_emp == null)
            m_emp = GameObject.Find("EMP").GetComponent<EMP>();
    }
	
	// Update is called once per frame
	void Update () {
        if(padding > 0)
            padding -= Time.deltaTime;

        UpdateInput();
	}
   
    private void UpdateInput() {
        if (Input.GetKey("c") && padding <= 0.0f) {
            if (m_playerData.GetCloakCooldown() <= 0.0f) {
                padding = 0.2f;
                m_playerData.SetCloaked(!m_playerData.GetCloaked());
            }      
        }

        if (Input.GetKey("x") && padding <= 0.0f) {
            if (m_emp.GetEmpCooldown() <= 0.0f && !m_emp.GetEmpActive()) {
                padding = 0.2f;
                m_emp.SetEmpActive(true);
            }
        }
    }
}