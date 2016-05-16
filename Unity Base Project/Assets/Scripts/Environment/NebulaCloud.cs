using UnityEngine;

public class NebulaCloud : MonoBehaviour {

    public float timer;
    private PlayerHealth playerHealth;
    private PlayerStats player;
    private GameObject message;

    // Use this for initialization
    void Start() {
        Vector3 m_scale = Vector3.zero;
        m_scale.x = Random.Range(10f, 20.0f);
        m_scale.y = m_scale.x;
        m_scale.z = m_scale.x;

        Vector3 newScale = transform.localScale;
        newScale.x *= m_scale.x;
        newScale.y *= m_scale.y;
        newScale.z *= m_scale.z;
        transform.localScale = newScale;

        timer = 0.0f;
        playerHealth = GameObject.Find("Health").GetComponent<PlayerHealth>();
        player = GameObject.Find("Player").GetComponent<PlayerStats>();
        message = GameObject.Find("WarningMessages");
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Collision
    public void OnTriggerStay(Collider col)
    {
        if (col.transform.tag == "Player")
        {
            timer += Time.deltaTime;
            message.SendMessage("Poison");
            if (col.GetComponentInChildren<PlayerStats>().GetShield())
            {
                if (timer >= 9.0f)
                {
                    player.EnvironmentalDMG();
                    timer = 0.0f;
                }
            }
            else
            {
                if (timer >= 9.0f)
                {
                    timer = 0.0f;
                    playerHealth.EnvironmentalDMG();

                }
               
            }
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.transform.tag == "Player")
        {
            message.SendMessage("NoPoison");
            timer = 0.0f;
        }
    }
    #endregion
}