using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerData : MonoBehaviour {
    //**    Attach Script to Player Gameobject   **//
    public bool gamePause;
    public bool isCloaked;



    public bool hyperDrive;
    public float hyperDriveStartTimer;


    public GameObject hyperDriveParticles;

    public int hitCount;

    private float padding;
    public float cloakTimer;
    public float cloakCooldown;


    public GameObject[] shipLights;

    private PlayerMovement m_playerMove;

    // Use this for initialization
    void Start() {
        hitCount = 0;
        isCloaked = false;
        cloakTimer = 0.0f;
        cloakCooldown = 0.0f;

        hyperDrive = false;

        if(shipLights.Length == 0)
            shipLights = GameObject.FindGameObjectsWithTag("ShipLights");

        if (hyperDriveParticles == null)
            hyperDriveParticles = GameObject.Find("HyperDriveParticles");

        hyperDriveParticles.SetActive(false);

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

            if (Input.GetKey(KeyCode.H))
                HyperDriveInitialize();

            if (hyperDrive)
            {
                HyperDriveMotherFucker();
            }
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

        if(col.name == "Loot")
        {
            m_playerMove.StopAllMovement();
            SceneManager.LoadScene("Win_Scene");
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

    public void HyperDriveMotherFucker()
    {
        if (hyperDriveStartTimer > 0.0f)
        {
            hyperDriveStartTimer -= Time.deltaTime;
            hyperDriveParticles.transform.Translate(Vector3.forward * 20.0f * Time.deltaTime);
        }
        else
        {
            Debug.Log("HYPERDRIVE Active!");
            transform.Translate(Vector3.forward * 200.0f);
            hyperDriveParticles.SetActive(false);            
            hyperDrive = false;
        }
    }
    public void SetCloaked(bool boolean) {
        if (boolean) {
            cloakTimer = 45.0f;
            for (int x = 0; x < shipLights.Length; x++)
                shipLights[x].gameObject.SetActive(false);            
        }
        else {
            cloakTimer = 0.0f;
            cloakCooldown = 60.0f;
            for (int x = 0; x < shipLights.Length; x++)
                shipLights[x].gameObject.SetActive(true);
        }
        isCloaked = boolean;
    }

    public void Hit()
    {
        hitCount++;
    }

    public void HyperDriveInitialize()
    {
        Debug.Log("Initializing Hyper drive...");
        hyperDriveParticles.SetActive(true);
        hyperDrive = true;
        hyperDriveStartTimer = 5.0f;
    }
    #endregion
}