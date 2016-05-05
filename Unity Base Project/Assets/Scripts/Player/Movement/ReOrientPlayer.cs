using UnityEngine;
using System.Collections;

public class ReorientPlayer : MonoBehaviour {
    //**    Attach to Player blip on Radar  **//
    private GameObject Player;


    // Use this for initialization
    void Start() {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Vector3 rot = Player.transform.localEulerAngles;
        rot.x -= 180;
        transform.localEulerAngles = rot;
    }

    void OnTriggerEnter(Collider col) {
        if(col.name == "bone3")
            Player.SendMessage("ResetOrientation");        
    }
}
