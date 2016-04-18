using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerData : MonoBehaviour {
    //**    Attach Script to Player Gameobject   **//
    public bool gamePause;
    public bool isCloaked;

    public int hitCount;

    public float cloakTimer;
    public float cloakCooldown;

    private float padding;

    private PlayerMovement m_playerMove;

    // Use this for initialization
    void Start() {
        hitCount = 0;
        SetCloaked(false);
        cloakTimer = 0.0f;
        cloakCooldown = 0.0f;

        if (m_playerMove == null)
            m_playerMove = this.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update() {
        if (hitCount > 1)
            SceneManager.LoadScene("Credits_Scene");

        if (!GetGamePaused())
        {
            if (padding > 0)
                padding -= Time.deltaTime;

            if (cloakCooldown > 0)
                cloakCooldown -= Time.deltaTime;

            if (cloakTimer > 0)
                cloakTimer -= Time.deltaTime;
            else if (cloakTimer < 0)
                SetCloaked(false);


            if (Input.GetKey(KeyCode.X) && padding <= 0.0f)
            {
                padding = 0.2f;
                SetCloaked(!GetCloaked());
            }
        }
    }

    #region Collision Detection
    void OnTriggerEnter(UnityEngine.Collider col) {
        if (col.name == "Enemy") {
            hitCount++;
            m_playerMove.StopAllMovement();
            Debug.Log("Collided with " + col.name);
        }

        if (col.tag == "Asteroid" && m_playerMove.GetDriveMode()) {
            hitCount++;
            m_playerMove.StopAllMovement();
            Debug.Log("Collided with " + col.name);
        }
    }
    #endregion

    #region Accessors
    public bool GetGamePaused() {
        return gamePause;
    }
    public bool GetCloaked() {
        return isCloaked;
    }
    public float GetCloakTimer() {
        return cloakTimer;
    }
    public float GetCloakCooldown() {
        return cloakCooldown;
    }
    #endregion

    #region Modifiers
    public void SetGamePause(bool boolean) {
        gamePause = boolean;
    }
    public void SetCloaked(bool boolean) {
        if (boolean) 
            cloakTimer = 45.0f;        
        else {
            cloakTimer = 0.0f;
            cloakCooldown = 60.0f;
        }
        isCloaked = boolean;
    }
    #endregion
}