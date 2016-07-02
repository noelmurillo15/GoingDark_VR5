using UnityEngine;
using GoingDark.Core.Enums;
using UnityEngine.UI;

public class WeaponSelectButton : MonoBehaviour
{

    #region Properties
    private MissileType Type { get; set; }
    private float transition;
    private Image m_button;
    private Color original;
    private SystemManager manager;
    private bool wFlip;
    #endregion


    // Use this for initialization
    void Start()
    {
        wFlip = false;
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
                Type = MissileType.Basic;
                break;
            case "EmpMissile":
                Type = MissileType.Emp;
                break;
            case "ChromaticMissile":
                Type = MissileType.Chromatic;
                break;
            case "ShieldBreakerMissile":
                Type = MissileType.ShieldBreak;
                break;
        }
    }


    #region Collision
    public void OnTriggerEnter(Collider col)
    {
        if (col.name == "bone3" && m_button.color == original)
        {
            transition = 0.2f;
            m_button.color = Color.green;
            wFlip = true;
        }
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.name == "bone3" && wFlip)
        {
            transition -= Time.deltaTime;
            if (transition <= 0.0f && m_button.color == Color.green)
            {
                wFlip = false;
            }
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.name == "bone3")
            m_button.color = original;
    }
    #endregion    
}