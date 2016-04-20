using UnityEngine;
using UnityEngine.UI;

public class CrewMenu : MonoBehaviour {

    public Image m_button;
    public float padding;
    public EMP emp;
    public GameObject mMenu;
    public PlayerData playerData;
    public ShootObject missileLauncher;

    public ArmSettings missileCam;

    // Use this for initialization
    void Start()
    {
        padding = 0.0f;

        if (playerData == null)
            playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();

        if (emp == null)
            emp = GameObject.Find("EMP").GetComponent<EMP>();

        if (missileLauncher == null)
            missileLauncher = GameObject.Find("PlayerGun").GetComponent<ShootObject>();

        if (mMenu == null)
            mMenu = GameObject.Find("Tactician_Menu");            
    }

    void Awake()
    {
        m_button = GetComponent<Image>();
        m_button.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        if (padding > 0.0f)
            padding -= Time.deltaTime;

        if (playerData.GetCloakCooldown() > 0.0f) { 
            if (m_button.name == "CloakButton")
                m_button.color = Color.red;
        }        

        if (emp.GetEmpCooldown() > 0.0f) { 
            if (m_button.name == "EMPButton")
                m_button.color = Color.red;
        }

        if (missileLauncher.GetFireCooldown() > 0.0f) {
            if (m_button.name == "FireButton")
                m_button.color = Color.red;
        }
    }

    #region Collision
    public void OnTriggerEnter(Collider col)
    {
        if (col.name == "bone3")
        {
            if (col.transform.parent.name == "leftIndex" || col.transform.parent.name == "rightIndex")
            {
                m_button.color = Color.green;
            }
        }
    }

    public void OnTriggerExit(Collider col){
        if (col.name == "bone3" && padding <= 0.0f)
        {
            if (transform.name == "EMPButton" && emp.GetEmpCooldown() <= 0.0f)
                emp.SetEmpActive(true);
            else if (transform.name == "CloakButton" && playerData.GetCloakCooldown() <= 0.0f)
            {
                if (!playerData.GetCloaked())
                    playerData.SetCloaked(true);
                else if (playerData.GetCloaked())
                    playerData.SetCloaked(false);
            }
            else if (transform.name == "FireButton" && missileLauncher.GetFireCooldown() <= 0.0f)
            {
                if (missileCam == null)
                    missileCam = GameObject.Find("leftForearm").GetComponent<ArmSettings>();

                missileCam.TurnCamOn();
                missileLauncher.FireMissile();
            }
            
            padding = 0.2f;
            mMenu.SetActive(false);
        }
    }
    #endregion
}