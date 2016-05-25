using UnityEngine;
using GD.Core.Enums;
using UnityEngine.UI;

public class WeaponSelectButton : MonoBehaviour
{

    #region Properties
    private MissileType Type { get; set; }
    private float transition;
    private Image m_button;
    private Color original;
    private SystemManager manager;
    #endregion


    // Use this for initialization
    void Start()
    {
        transition = 0f;
        m_button = GetComponent<Image>();
        original = m_button.color;
        Initialize();
        manager = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemManager>();
    }

    private void Initialize()
    {
        switch (transform.name)
        {
            case "BasicMissile":
                Type = MissileType.BASIC;
                break;
            case "EmpMissile":
                Type = MissileType.EMP;
                break;
            case "ChromaticMissile":
                Type = MissileType.CHROMATIC;
                break;
            case "ShieldBreakerMissile":
                Type = MissileType.SHIELDBREAKER;
                break;
        }
    }

    void ActivateButton()
    {    
        manager.MissileSelect(Type);
    }


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