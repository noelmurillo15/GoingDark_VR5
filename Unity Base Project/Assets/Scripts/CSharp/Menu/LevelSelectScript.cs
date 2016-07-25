using UnityEngine;
using UnityEngine.UI;

public class LevelSelectScript : MonoBehaviour {

    private Image m_button;
    private float transition;
    private float cancelTimer;
    private PlayerViewCheck player;

    // Use this for initialization
    void Start () {
        transition = 0.0f;
        cancelTimer = 0.0f;
        m_button = GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerViewCheck>();
    }
    
    // Update is called once per frame
    void Update () {
        
    }

    void Accept()
    {
        player.isSwitching = true;
    }
}
