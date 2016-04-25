using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerShipData : MonoBehaviour {

    private int hitCount;
    private JoyStickMovement m_playerMove;

    // Use this for initialization
    void Start () {
        hitCount = 0;
        m_playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<JoyStickMovement>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    #region Accessors
    public int GetHitCount() {
        return hitCount;
    }
    #endregion

    #region Modifiers
    public void IncreaseHitCount() {
        hitCount++;
    }

    public void DecreaseHitCount() {
        hitCount--;
    }
    #endregion

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Asteroid")
        {
            Debug.Log("Ship Colliding with " + col.transform.tag);
            Crash();
        }
    }

    #region Msg Calls
    public void Hit() {
        Debug.Log("Hit by enemy Missile");
        IncreaseHitCount();
        m_playerMove.StopMovement();

        if (hitCount > 2)
            SceneManager.LoadScene("Game_Over");
    }

    public void Crash() {
        Debug.Log("Crashed into Asteroid");
        m_playerMove.StopMovement();
    }
    #endregion
}
