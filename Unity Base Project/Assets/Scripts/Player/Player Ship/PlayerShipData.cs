using UnityEngine;

public class PlayerShipData : MonoBehaviour {
    //**    Attach to Player's ship **//
    
    //  Utils
    private EMP m_Emp;
    private Cloak m_Cloak;
    private HyperDrive m_Hyperdrive;
    private PlayerMovement m_playerMove;

    // Use this for initialization
    void Start () {
        
        m_playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        m_Cloak = GameObject.Find("Cloak").GetComponent<Cloak>();        
        m_Hyperdrive = GameObject.Find("HyperDrive").GetComponent<HyperDrive>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    #region Accessors    
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
}