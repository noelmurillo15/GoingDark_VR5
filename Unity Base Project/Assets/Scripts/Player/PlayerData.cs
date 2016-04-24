using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerData : MonoBehaviour {
    //**    Attach Script to Player Gameobject   **// 

    public int hitCount;
    
    private Cloak playerCloak;
    private HyperDrive playerHyperdrive;
    private JoyStickMovement m_playerMove;
    

    // Use this for initialization
    void Start() {
        hitCount = 0;

        if (m_playerMove == null)
            m_playerMove = this.GetComponent<JoyStickMovement>();

        if (playerCloak == null)
            playerCloak = GameObject.Find("Cloak").GetComponent<Cloak>();

        if (playerHyperdrive == null)
            playerHyperdrive = GameObject.Find("HyperDrive").GetComponent<HyperDrive>();
    }

    // Update is called once per frame
    void Update() {
        
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

        if (hitCount > 2)
            SceneManager.LoadScene("Game_Over");
    }

    public void Crash()
    {
        Debug.Log("Crashed into Asteroid");
        m_playerMove.StopMovement();
    }
    #endregion
}