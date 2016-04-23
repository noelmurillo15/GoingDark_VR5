using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerData : MonoBehaviour {
    //**    Attach Script to Player Gameobject   **//
    public bool gamePause;   

    public int hitCount;

    private float padding;
    
    public GameObject[] shipLights;

    private JoyStickMovement m_playerMove;

    private Cloak playerCloak;
    private HyperDrive playerHyperdrive;
    

    // Use this for initialization
    void Start() {
        hitCount = 0;

        if (playerCloak == null)
            playerCloak = GameObject.Find("Cloak").GetComponent<Cloak>();

        if (playerHyperdrive == null)
            playerHyperdrive = GameObject.Find("HyperDrive").GetComponent<HyperDrive>();

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
            
        }
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
    }
    #endregion

    #region Accessors
    public bool GetGamePaused() {
        return gamePause;
    }
    
    public Cloak GetPlayerCloak()
    {
        return playerCloak;
    }
    public HyperDrive GetPlayerHyperDrive()
    {
        return playerHyperdrive;
    }

    #endregion

    #region Modifiers
    public void SetGamePause(bool boolean) {
        gamePause = boolean;
    }

    public void Hit()
    {
        hitCount++;
        m_playerMove.StopMovement();
    }

    public void Crash()
    {
        m_playerMove.StopMovement();
    }
    #endregion
}