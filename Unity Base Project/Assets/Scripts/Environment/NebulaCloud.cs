using UnityEngine;

public class NebulaCloud : MonoBehaviour {

    public float timer;
    private PlayerStats player;

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
        player = GameObject.Find("Player").GetComponent<PlayerStats>();
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
            if (!col.GetComponentInChildren<PlayerStats>().GetShield())
            {
                timer += Time.deltaTime;
                if (timer <= 100.0f)
                {
                    player.EnvironmentalDMG();
                    timer = 0.0f;
                }
            }
            else
                timer = 0.0f;
            
        }        
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.transform.tag == "Player")
        {
            timer = 0.0f;
        }
    }
    #endregion
}