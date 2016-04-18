using UnityEngine;
using UnityEngine.UI;

public class CrewMenu : MonoBehaviour
{

    private Image m_button;
    private float transition;
    private float cancelTimer;
    private PlayerData playerData;
    private EMP emp;

    // Use this for initialization
    void Start()
    {
        transition = 0.0f;
        cancelTimer = 0.0f;
        m_button = GetComponent<Image>();

        if (playerData == null)
            playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();

        if (emp == null)
            emp = GameObject.Find("EMP").GetComponent<EMP>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Collision
    public void OnTriggerEnter(Collider col)
    {
        if (col.name == "bone3")
        {
            if (col.transform.parent.name == "leftIndex" || col.transform.parent.name == "rightIndex")
            {
                transition = 0.1f;
                cancelTimer = 1.25f;
                m_button.color = Color.white;
                m_button.CrossFadeColor(Color.green, 0.1f, false, false);
            }
        }
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.name == "bone3")
        {            
            if (col.transform.parent.name == "leftIndex" || col.transform.parent.name == "rightIndex")
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
    }

    public void OnTriggerExit(Collider col){
        if (col.name == "bone3"){
            if (transform.name == "EMPButton"){
                if (emp.GetEmpCooldown() <= 0.0f)
                {
                    Debug.Log("EMP...");
                    emp.SetEmpActive(true);
                }
                else
                    m_button.color = Color.red;
            }
            else if (transform.name == "CloakButton"){
                if (playerData.GetCloakCooldown() <= 0.0f && playerData.isCloaked == false){
                    Debug.Log("Cloaking...");
                    playerData.SetCloaked(true);
                }
                else if (playerData.isCloaked){
                    Debug.Log("Un-Cloaking...");
                    playerData.SetCloaked(false);                    
                }
            }
        }
        m_button.color = Color.white;
        m_button.CrossFadeColor(Color.white, 0.01f, false, false);
    }
    #endregion
}