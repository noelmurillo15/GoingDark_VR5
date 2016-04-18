using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerData : MonoBehaviour {
    //**    Attach Script to Player Gameobject   **//
    public bool gamePause;
    public bool isCloaked;

    public int hitCount;

    public float cloakTimer;
    public float cloakCooldown;

    private float padding;

    public GameObject[] shipLights;

    private PlayerMovement m_playerMove;

    // Use this for initialization
    void Start() {
        hitCount = 0;
        SetCloaked(false);
        cloakTimer = 0.0f;
        cloakCooldown = 0.0f;

        if(shipLights.Length == 0)
            shipLights = GameObject.FindGameObjectsWithTag("ShipLights");


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
        }
    }

    #region Collision Detection
    void OnTriggerEnter(Collider col) {
        if (col.name == "Enemy") {
            hitCount++;

            for (int x = 0; x < shipLights.Length; x++)
                shipLights[x].GetComponent<Light>().color = Color.red;

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
        {
            Debug.Log("Cloaking...");
            cloakTimer = 45.0f;
            for (int x = 0; x < shipLights.Length; x++)
            {
                shipLights[x].gameObject.SetActive(false);
                Debug.Log("Cloaked");
            }
        }
        else
        {
            Debug.Log("Un-Cloaking...");
            cloakTimer = 0.0f;
            cloakCooldown = 60.0f;
            for (int x = 0; x < shipLights.Length; x++)
            {
                shipLights[x].gameObject.SetActive(true);
                Debug.Log("Un-Cloaked");
            }
        }
        isCloaked = boolean;
    }
    #endregion
}