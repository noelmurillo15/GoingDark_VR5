using UnityEngine;

public class EMP : MonoBehaviour
{
    #region Properties
    public bool Activated { get; private set; }
    public float Cooldown { get; private set; }

    public float empTimer;
    private GameObject shockwave;
    #endregion


    // Use this for initialization
    void Start()
    {
        empTimer = 0f;
        Cooldown = 0f;
        Activated = false;

        shockwave = transform.GetChild(0).gameObject;
        shockwave.SetActive(Activated);
    }

    // Update is called once per frame
    void Update()
    {
        if (Cooldown > 0f)
            Cooldown -= Time.deltaTime;

        if (empTimer > 0f)
            empTimer -= Time.deltaTime;
        else
        {
            if (Activated)
                SetEmpActive(false);
        }

        if (Input.GetKey(KeyCode.E))
            SetEmpActive(true);
    }


    #region Modifiers
    public void SetEmpActive(bool boolean)
    {
        if (empTimer <= 0f && Cooldown <= 0f)
        {
            if (boolean)
                empTimer = 5f;
            else
                Cooldown = 5f;

            Activated = boolean;
            shockwave.SetActive(Activated);
        } 
    }
    #endregion
}