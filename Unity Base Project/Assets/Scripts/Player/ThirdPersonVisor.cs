using Leap;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThirdPersonVisor : MonoBehaviour {
    //**    Attach Script to Leap Control   **//

    private GameObject visorHUD;    
    private GameObject missileWarning;
    private GameObject uTurn;   
    

    


    // Use this for initialization
    void Start() {

        if (visorHUD == null)
            visorHUD = GameObject.Find("VisorHUD");        

        if (uTurn == null)
            uTurn = GameObject.Find("uTurn");

        if (missileWarning == null)
            missileWarning = GameObject.Find("incomingMissile");

        

        visorHUD.SetActive(true);
        missileWarning.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
            
    }

    public void SetIncomingMissileWarning(bool boolean)
    {
        missileWarning.SetActive(boolean);
    }

    public void SetUTurnWarning(bool boolean)
    {
        uTurn.SetActive(boolean);
    }
}
