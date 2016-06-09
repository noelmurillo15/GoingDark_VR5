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

    public void OnTriggerEnter(Collider col)
    {
        if (col.name == "bone3" && (col.transform.parent.name == "rightIndex"|| col.transform.parent.name == "leftIndex"))
        {
            m_button.color = Color.red;
            transition = 0.1f;
            cancelTimer = 2f;
            m_button.CrossFadeColor(Color.green, 0.1f, false, false);
        }
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.name == "bone3" && (col.transform.parent.name == "rightIndex" || col.transform.parent.name == "leftIndex"))
        {
            transition -= Time.deltaTime;
            cancelTimer -= Time.deltaTime;

            if (transition <= 0.0f)
            {
                m_button.CrossFadeColor(Color.white, 0.01f, false, false);
                m_button.color = Color.green;
            }

            if (cancelTimer <= 0.0f)
                m_button.color = Color.red;
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.name == "bone3" && (col.transform.parent.name == "rightIndex" || col.transform.parent.name == "leftIndex"))
        {
            if (m_button.color == Color.green)
            {
                player.isSwitching = true;

            }
            else {
                m_button.color = Color.white;
                m_button.CrossFadeColor(Color.white, 0.01f, false, false);
            }
        }
    }
}
