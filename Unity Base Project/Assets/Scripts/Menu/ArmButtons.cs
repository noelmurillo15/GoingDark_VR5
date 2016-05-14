using UnityEngine;
using GD.Core.Enums;
using UnityEngine.UI;

public class ArmButtons : MonoBehaviour
{

    #region Properties
    public SystemType Type { get; private set; }
    private float delay;
    private float transition;
    private float cancelTimer;


    private Image m_button;    
    private SystemsManager manager;
    #endregion


    // Use this for initialization
    void Start()
    {
        Initialize();
        delay = 10f;
        transition = 0f;
        cancelTimer = 0f;
        m_button = GetComponent<Image>();
        manager = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemsManager>();
    }

    void Update()
    {
        if (delay > 0f)
            delay -= Time.deltaTime;
        else
        {
            delay = .5f;
        }
    }

    #region Private Methods
    void Initialize()
    {
        switch (transform.name)
        {
            case "Emp":
                Type = SystemType.EMP;
                break;
            case "Cloak":
                Type = SystemType.CLOAK;
                break;
            case "Hyperdrive":
                Type = SystemType.HYPERDRIVE;
                break;
            case "Missile":
                Type = SystemType.MISSILES;
                break;
            case "Decoy":
                Type = SystemType.DECOY;
                break;


            default:
                Debug.Log("Init : " + transform.name);
                break;
        }
    }
    private void ActivateButton()
    {
        manager.SendMessage("ActivateSystem", Type);
        m_button.color = Color.white;
    }
    #endregion

    #region Collision
    public void OnTriggerEnter(Collider col)
    {
        if (col.name == "bone3")
        {
            transition = 0.1f;
            cancelTimer = 1.5f;
            m_button.color = Color.grey;
        }
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.name == "bone3")
        {
            if (cancelTimer > 0.0f)
            {
                transition -= Time.deltaTime;
                cancelTimer -= Time.deltaTime;

                if (transition <= 0.0f && m_button.color == Color.grey)
                    m_button.color = Color.green;
            }
            else
                m_button.color = Color.red;
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.name == "bone3")
            if (m_button.color == Color.green)
                ActivateButton();
    }
    #endregion    
}