using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemyScript : MonoBehaviour {

    private float min, max;
    private float loseSpeed;

    //  Player Info
    public float zOffset;
    public GameObject m_Player;
    public PlayerData m_playerInput;

    public GameObject explosion;
    // Use this for initialization
    void Start () {
	
	}

    void Awake() {
        min = 50.0f;
        max = 100.0f;
        loseSpeed = -50.0f;
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_playerInput = m_Player.GetComponent<PlayerData>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!m_playerInput.GetCloaked())
            zOffset = transform.position.z - m_Player.transform.position.z;

        if (zOffset < max && zOffset > min)
            LoseScene();              
	}

    private void LoseScene() {
        min = 50.0f;
        max = 100.0f;
        explosion.SetActive(true);        
        Debug.Log("Enemy Attack!");
        if (loseSpeed < 0)
            loseSpeed += 25.0f * Time.deltaTime;
        else 
            SceneManager.LoadScene("Credits_Scene");        
    }
}