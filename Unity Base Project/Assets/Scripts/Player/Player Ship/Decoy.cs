using UnityEngine;

public class Decoy : MonoBehaviour {
    //**    Attach to Decoy GameObject  **//
    private float aliveTimer;


    // Use this for initialization
    void Start() {
        aliveTimer = 30.0f;
    }

    // Update is called once per frame
    void Update() {
        if (aliveTimer > 0.0f)
            aliveTimer -= Time.deltaTime;
        else
            Destroy(this.gameObject);
    }

    #region Modifiers
    #endregion

    #region Accessors
    #endregion

    #region Msg Calls
    public void Kill() {
        Destroy(this.gameObject);
    }
    #endregion
}