using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerData : MonoBehaviour {
    //**    Attach Script to Player Gameobject   **//
    public bool gamePause;
    public bool isCloaked;

    public bool loot1Found;
    public bool loot2Found;
    public bool loot3Found;



    public bool hyperDrive;
    public float hyperDriveTimer;
    public float hyperDriveStartTimer;

    public Vector3 particleOriginalPos;


    private GameObject hyperDriveParticles;

    public int hitCount;

    private float padding;
    public float cloakTimer;
    public float cloakCooldown;


    public GameObject[] shipLights;

    private JoyStickMovement m_playerMove;

    // Use this for initialization
    void Start() {
        hitCount = 0;
        isCloaked = false;
        cloakTimer = 0.0f;
        cloakCooldown = 0.0f;

        hyperDrive = false;
        loot1Found = false;
        loot2Found = false;
        loot3Found = false;

        if (shipLights.Length == 0)
            shipLights = GameObject.FindGameObjectsWithTag("ShipLights");

        if (hyperDriveParticles == null)
            hyperDriveParticles = GameObject.Find("HyperDriveParticles");

        particleOriginalPos = hyperDriveParticles.transform.localPosition;
        hyperDriveParticles.SetActive(false);

        if (m_playerMove == null)
            m_playerMove = this.GetComponent<JoyStickMovement>();
    }

    // Update is called once per frame
    void Update() {
        if (hitCount > 2)
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
                HyperDriveMotherFucker();
            
        }

        if (loot1Found && loot2Found && loot3Found)
            SceneManager.LoadScene("Win_Scene");
    }

    #region Collision Detection
    void OnTriggerEnter(Collider col) {
        if (col.name == "Enemy") {
            hitCount++;

            for (int x = 0; x < shipLights.Length; x++)
                shipLights[x].GetComponent<Light>().color = Color.red;

            m_playerMove.StopMovement();
            Debug.Log("Collided with " + col.name);
        }

        if(col.name == "Loot1")
            loot1Found = true;
        if (col.name == "Loot2")
            loot2Found = true;
        if (col.name == "Loot3")
            loot3Found = true;
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
            hyperDriveParticles.transform.Translate(Vector3.forward * 10.0f * Time.deltaTime);
            hyperDriveTimer = 0.5f;
            m_playerMove.SetMoveSpeed(0.0f);
        }
        else
        {
            if (hyperDriveTimer > 0.0f)
            {
                hyperDriveTimer -= Time.deltaTime;
                transform.Translate(Vector3.forward * 800.0f * Time.deltaTime);
            }
            else
            {
                hyperDriveParticles.transform.localPosition = particleOriginalPos;
                hyperDriveParticles.SetActive(false);
                hyperDrive = false;
            }
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
        m_playerMove.StopMovement();
    }

    public void HyperDriveInitialize()
    {
        m_playerMove.SetMoveSpeed(0.0f);
        hyperDriveParticles.SetActive(true);
        hyperDrive = true;
        hyperDriveStartTimer = 5.0f;
    }
    #endregion
}