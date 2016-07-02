using UnityEngine;

public class GameplayManager : MonoBehaviour {

    // Use this for initialization
    public bool isPaused;
    private float padding;


    void Start() {
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (padding > 0.0f)
            padding -= Time.deltaTime;

        if (Input.GetKey(KeyCode.Escape) && padding <= 0.0f)
            SetGamePause(!GetGamePaused());
    }

    #region Accessors
    public bool GetGamePaused() {
        return isPaused;
    }
    #endregion

    #region Modifiers
    public void SetGamePause(bool boolean) {
        isPaused = boolean;
        padding = 0.2f;
    }
    #endregion
}
