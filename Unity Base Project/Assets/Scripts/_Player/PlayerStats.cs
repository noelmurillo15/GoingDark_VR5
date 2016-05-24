using UnityEngine;
using GD.Core.Enums;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour {
    //**    Attach to Player    **//

    #region Properties
    public Impairments Debuff { get; private set; }
    private MovementStats MoveData;
    private SystemManager Systems;

    //  Credits
    public int numCredits;

    //  Current Sector Name
    public string sectorName;

    //  Shields
    private bool shieldOn;
    private float shieldHealth;
    private GameObject shield;

    //  Rotation
    private Transform MyTransform;
    private float horizontal, vertical;
    private CharacterController controller;
    private bool xaxis;
    Quaternion qX, qY;
    #endregion


    // Use this for initialization
    void Start () {
        qX = Quaternion.identity;
        qY = Quaternion.identity;
        xaxis = false;
        MoveData.Speed = 0f;
        MoveData.Boost = 1f;
        MoveData.MaxSpeed = 100f;
        MoveData.RotateSpeed = 25f;
        MoveData.Acceleration = 50f;
        numCredits = PlayerPrefs.GetInt("Credits");
        Systems = GameObject.Find("Devices").GetComponent<SystemManager>();

        MyTransform = transform;
        vertical = MyTransform.eulerAngles.x;
        horizontal = MyTransform.eulerAngles.y;

        // shield defaults
        shieldOn = false;
        shieldHealth = 3;
        shield = GameObject.FindGameObjectWithTag("Shield");
    }
	
	// Update is called once per frame
	void Update() {
        // shield cooldown and reactivate
        if (shieldHealth != 0.0f)
            shieldHealth += Time.deltaTime;

        qX = Quaternion.identity;
        qY = Quaternion.identity; 

        if (Input.GetAxis("LTrigger") > 0f)
            IncreaseSpeed();
        else
            DecreaseSpeed();

        if (Input.GetAxis("Horizontal") > 0f)
        {
            TurnLeft();
        }
        if (Input.GetAxis("Horizontal") < 0f)
        {
            TurnRight();
        }

        if (Input.GetAxis("Vertical") > 0f)
        {
            GoUp();            
        }
        if (Input.GetAxis("Vertical") < 0f)
        {
            GoDown();
        }
    }

    #region Accessors
    public bool GetShield()
    {
        return shieldOn;
    }       
    public SystemManager GetSystems()
    {
        return Systems;
    }
    public MovementStats GetMoveData()
    {
        return MoveData;
    }
    #endregion

    #region Modifiers
    public void StopMovement()
    {
        MoveData.Speed = 0f;
    }
    public void IncreaseSpeed()
    {
        if (MoveData.Speed < (MoveData.MaxSpeed * MoveData.Boost))
            MoveData.Speed += Time.deltaTime * MoveData.Acceleration;
        else if (MoveData.Speed > (MoveData.MaxSpeed * MoveData.Boost) + .5f)
            DecreaseSpeed();
    }
    public void DecreaseSpeed()
    {
        if (MoveData.Speed > 0.0f)
            MoveData.Speed -= Time.deltaTime * MoveData.Acceleration * 5f;
        else
            MoveData.Speed = 0.0f;
    }
    public void TurnLeft()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * -MoveData.RotateSpeed);
        //horizontal -= MoveData.RotateSpeed * Time.deltaTime;
        //horizontal = Mathf.Repeat(horizontal, 360f);
        //qY = Quaternion.AngleAxis(horizontal, MyTransform.up);
        //horizontal -= MoveData.RotateSpeed * Time.deltaTime;
        //Quaternion rot = Quaternion.Euler(MyTransform.eulerAngles.x, horizontal, MyTransform.eulerAngles.z);
        //MyTransform.rotation = Quaternion.Lerp(MyTransform.rotation, rot, 1f);
    }
    public void TurnRight()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * MoveData.RotateSpeed);
        //horizontal += MoveData.RotateSpeed * Time.deltaTime;
        //horizontal = Mathf.Repeat(horizontal, 360f);
        //qY = Quaternion.AngleAxis(horizontal, MyTransform.up);
        //horizontal += MoveData.RotateSpeed * Time.deltaTime;
        //Quaternion rot = Quaternion.Euler(MyTransform.eulerAngles.x, horizontal, MyTransform.eulerAngles.z);
        //MyTransform.rotation = Quaternion.Lerp(MyTransform.rotation, rot, 1f);
    }

    public void GoUp()
    {
        transform.Rotate(Vector3.right * Time.deltaTime * MoveData.RotateSpeed);
        //vertical -= MoveData.RotateSpeed * Time.deltaTime;
        //vertical = Mathf.Repeat(vertical, 360f);
        //qX = Quaternion.AngleAxis(horizontal, MyTransform.right);
        //if (xaxis)
        //    MyTransform.RotateAround(MyTransform.right, vertical * .5f);
        //else
        //    MyTransform.RotateAround(MyTransform.right, vertical);
    }
    public void GoDown()
    {
        transform.Rotate(Vector3.right * Time.deltaTime * -MoveData.RotateSpeed);
        //vertical += MoveData.RotateSpeed * Time.deltaTime;
        //vertical = Mathf.Repeat(vertical, 360f);
        //qX = Quaternion.AngleAxis(horizontal, MyTransform.right);
        //if (xaxis)
        //    MyTransform.RotateAround(MyTransform.right, vertical * .5f);
        //else
        //    MyTransform.RotateAround(MyTransform.right, vertical);
    }
    #endregion

    #region Msg Functions
    public void UpdateSector(string _name)
    {
        sectorName = _name;
    }
    public void EMPHit()
    {
        Debug.Log("EMP has affected Player's Systems");
    }

    public void Hit()
    {
        Debug.Log("Player Has Been Hit");
        if (shieldOn)
        {
            AudioManager.instance.PlayShieldHit();
            shieldHealth -= 100;
            if (shieldHealth <= 0)
            {
                shieldHealth = 0;
                shieldOn = false;
                shield.SetActive(shieldOn);
            }
        }
        else
        {
            Systems.SystemDamaged();
            PlayerHealth m_Health = GameObject.Find("Health").GetComponent<PlayerHealth>();
            m_Health.Hit();
            AudioManager.instance.PlayHit();
        }
       }

    public void EnvironmentalDMG()
    {
        if (shieldOn)
        {
            AudioManager.instance.PlayShieldHit();
            shieldHealth -= 100;
            if (shieldHealth <= 0)
            {
                shieldHealth = 0;
                shieldOn = false;
                shield.SetActive(shieldOn);
            }
        }
        else
        {        
            Systems.SystemDamaged();
            PlayerHealth m_Health = GameObject.Find("Health").GetComponent<PlayerHealth>();
            m_Health.Hit();
        }


    }

    public void Kill()
    {
        Debug.Log("Destroyed Player Ship");
        SceneManager.LoadScene("GameOver");
    }
    #endregion    
}