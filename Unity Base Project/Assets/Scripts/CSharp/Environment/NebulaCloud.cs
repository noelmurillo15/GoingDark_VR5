using UnityEngine;

public class NebulaCloud : MonoBehaviour
{

    private PlayerStats player;
    private GameObject message;

    // Use this for initialization
    void Start()
    {
        Vector3 m_scale = Vector3.zero;
        m_scale.x = Random.Range(10f, 20.0f);
        m_scale.y = m_scale.x;
        m_scale.z = m_scale.x;

        Vector3 newScale = transform.localScale;
        newScale.x *= m_scale.x;
        newScale.y *= m_scale.y;
        newScale.z *= m_scale.z;
        transform.localScale = newScale;        
    }

    void OnBecameVisible()
    {
        enabled = true;
    }
    void OnBecameInvisible()
    {
        enabled = false;
    }

    #region Collision
    public void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if(player == null || message == null)
            {
                message = GameObject.Find("PlayerCanvas");
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
            }
            message.SendMessage("Poison");
            player.InvokeRepeating("EnvironmentalDMG", 10f, 5f);
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            message.SendMessage("NoPoison");
            player.CancelInvoke("EnvironmentalDMG");
        }
    }
    #endregion
}