using UnityEngine;
using GD.Core.Enums;
using UnityEngine.UI;
using System.Collections;

public class ArmButtons : MonoBehaviour
{

    #region Properties
    public SystemType Type { get; private set; }

    private float transition;
    private Image m_button;
    private Color original;
    private SystemManager manager;
    #endregion


    // Use this for initialization
    void Start()
    {        
        Initialize();        
        StartCoroutine(UpdateCooldowns());
    }

    void Update()
    {
        
    }    

    #region Private Methods
    void Initialize()
    {
        transition = 0f;
        m_button = GetComponent<Image>();        
        switch (transform.name)
        {
            case "EmpButton":
                Type = SystemType.EMP;
                break;
            case "CloakButton":
                Type = SystemType.CLOAK;
                break;
            case "HyperdriveButton":
                Type = SystemType.HYPERDRIVE;
                break;
            case "MissileButton":
                Type = SystemType.MISSILES;
                break;
            case "LaserButton":
                Type = SystemType.LASERS;
                break;
            case "DecoyButton":
                Type = SystemType.DECOY;
                break;
            case "RadarButton":
                Type = SystemType.RADAR;
                break;


            default:
                Type = SystemType.NONE;
                break;
        }
        original = m_button.color;
        manager = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemManager>();
    }
    private void ActivateButton()
    {
        m_button.color = original;
        //AudioManager.instance.PlayMenuGood();
        if (Type == SystemType.RADAR || Type == SystemType.SHIELD)
        {
            manager.ToggleSystem(Type);
            return;
        }
        if (Type != SystemType.NONE)
        {
            manager.ActivateSystem(Type);
            return;
        }

        if (transform.name == "MissionLogButton")
            manager.ToggleMissionLog();
        else if (transform.name == "WeaponSelectButton")
            manager.ToggleWeaponSelect();
    }
    #endregion

    #region Coroutine
    private IEnumerator UpdateCooldowns()
    {
        while (true)
        {
            CooldownCheck();
            yield return new WaitForSeconds(2);
        }
    }
    private void CooldownCheck()
    {
        if (manager.GetSystemCooldown(Type) > 0f)
            m_button.color = Color.red;
        else
            m_button.color = original;
    }
    #endregion

    #region Collision
    public void OnTriggerEnter(Collider col)
    {
        if (col.name == "bone3" && m_button.color == original)
        {
            transition = 0.2f;
            m_button.color = Color.green;
        }
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.name == "bone3")
        {
            transition -= Time.deltaTime;
            if (transition <= 0.0f && m_button.color == Color.green)
                ActivateButton();
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.name == "bone3")
            m_button.color = original;
    }
    #endregion    
}