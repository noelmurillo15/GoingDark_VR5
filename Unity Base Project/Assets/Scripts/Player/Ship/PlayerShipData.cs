using UnityEngine;

public class PlayerShipData : MonoBehaviour {
    //**    Attach to Player's ship **//

    //  Disabilities
    public bool isStunned;

    //  Timers
    public float stunTimer;

    //  Utils
    private EMP m_Emp;
    private Cloak m_Cloak;
    private HyperDrive m_Hyperdrive;
    private PlayerStats m_PlayerStats;

    // Use this for initialization
    void Start () {
        stunTimer = 0f;
        isStunned = false;

        m_Emp = GameObject.Find("EMP").GetComponent<EMP>();
        m_Cloak = GameObject.Find("Cloak").GetComponent<Cloak>();        
        m_Hyperdrive = GameObject.Find("HyperDrive").GetComponent<HyperDrive>();
        m_PlayerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }
	
	// Update is called once per frame
	void Update () {
        if (stunTimer > 0f)
        {
            stunTimer -= Time.deltaTime;
            m_PlayerStats.DecreaseSpeed();
        }
        else
        {
            stunTimer = 0f;
            isStunned = false;
        }	
	}

    #region Accessors    
    public bool GetIsStunned()
    {
        return isStunned;
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
    public void SetIsStunned(bool boolean)
    {
        if (boolean)
            stunTimer = 5f;

        isStunned = boolean;
    }
    #endregion
}