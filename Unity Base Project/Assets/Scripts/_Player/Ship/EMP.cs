using UnityEngine;

public class EMP : MonoBehaviour
{
    //**    Attach to EMP Sphere    **/
    public bool isEmpActive;
    public float empTimer;
    public float empCooldown;
    private GameObject shockwave;


    // Use this for initialization
    void Start()
    {
        empTimer = 0f;
        empCooldown = 0f;
        isEmpActive = false;

        shockwave = transform.GetChild(0).gameObject;
        shockwave.SetActive(isEmpActive);
    }

    // Update is called once per frame
    void Update()
    {
        if (empCooldown > 0f)
            empCooldown -= Time.deltaTime;

        if (empTimer > 0f)
            empTimer -= Time.deltaTime;
        else
        {
            if (isEmpActive)
                SetEmpActive(false);
        }

        if (Input.GetKey(KeyCode.E))
            SetEmpActive(true);
    }


    #region Accessors
    public bool GetEmpActive()
    {
        return isEmpActive;
    }

    public float GetEmpCooldown()
    {
        return empCooldown;
    }
    #endregion

    #region Modifiers
    public void SetEmpActive(bool boolean)
    {
        if (empTimer <= 0f && empCooldown <= 0f)
        {
            if (boolean)
                empTimer = 5f;
            else
                empCooldown = 5f;

            isEmpActive = boolean;
            shockwave.SetActive(isEmpActive);
        } 
    }
    #endregion
}