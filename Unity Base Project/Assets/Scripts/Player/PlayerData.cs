using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerData : MonoBehaviour {
    //**    Attach Script to Player Gameobject   **// 

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

    }

    #region Accessors    
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

    #endregion

    #region Msg Calls
    public void Hit()
    {
        Debug.Log("Hit by enemy Missile");
        hitCount++;
        m_playerMove.StopMovement();
    }

    public void Crash()
    {
        Debug.Log("Crashed into Asteroid");
        m_playerMove.StopMovement();
    }
    #endregion
}