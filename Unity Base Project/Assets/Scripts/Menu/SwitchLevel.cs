using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchLevel : MonoBehaviour {

    public float m_timer;
    public string m_levelName;

    private bool m_bCountdown;
    private float m_timeRemaining;

	// Use this for initialization
	void Start () {
        m_timeRemaining = m_timer;
        m_bCountdown = true;                // On by default
	}

    void Awake()
    {
        m_timer = 5.0f;
    }
	
	// Update is called once per frame
	void Update () {
        if(m_bCountdown)
            m_timeRemaining -= Time.deltaTime;

        if (m_timeRemaining < 0.0f)
            SceneManager.LoadScene(m_levelName);        
	}

    public void EnableCountdown(bool bEnable, bool bReset) {
        m_bCountdown = bEnable;
		if(bReset)
            m_timeRemaining = m_timer;
    }

	public void SetDelay(float fDelay) {
		m_timer = fDelay;
	}

	public void SetDelayRemaining(float fDelay) {
        m_timeRemaining = fDelay;
	}
}
