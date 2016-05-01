using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerShipData : MonoBehaviour {
    //**    Attach to Player's ship **//
    private int hitCount;

    //  Utils
    private EMP m_Emp;
    private Cloak m_Cloak;
    private PlayerHealth m_Health;
    private HyperDrive m_Hyperdrive;
    private PlayerMovement m_playerMove;

    // Use this for initialization
    void Start () {
        hitCount = 0;
        m_playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        m_Cloak = GameObject.Find("Cloak").GetComponent<Cloak>();
        m_Health = GameObject.Find("Health").GetComponent<PlayerHealth>();
        m_Hyperdrive = GameObject.Find("HyperDrive").GetComponent<HyperDrive>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    #region Accessors
    public int GetHitCount() {
        return hitCount;
    }

    public EMP GetPlayerEMP() {
        return m_Emp;
    }
    public Cloak GetPlayerCloak() {
        return m_Cloak;
    }
    public HyperDrive GetPlayerHyperDrive() {
        return m_Hyperdrive;
    }
    #endregion

    #region Modifiers
    public void IncreaseHitCount() {
        hitCount++;
    }

    public void DecreaseHitCount() {
        hitCount--;
    }
    #endregion    

    #region Msg Calls
    public void Hit() {
        IncreaseHitCount();
        m_playerMove.StopMovement();
        m_Health.UpdatePlayerHealth();

        if (hitCount > 2)
            SceneManager.LoadScene("Game_Over");
    }

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag == "Asteroid")
            Hit();
    }
    #endregion
}
