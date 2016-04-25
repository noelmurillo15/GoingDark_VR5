using UnityEngine;

public class PlayerData : MonoBehaviour {
    //**    Attach Script to Player Gameobject   **// 

    private Cloak playerCloak;
    private HyperDrive playerHyperdrive;
        

    // Use this for initialization
    void Start() {
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
}