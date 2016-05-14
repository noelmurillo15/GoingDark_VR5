using UnityEngine;
using GD.Core.Enums;

public class EmpSystem : MonoBehaviour
{
    #region Properties
    public bool Activated { get; private set; }
    public float Cooldown { get; private set; }

    private float empTimer;
    private GameObject shockwave;

    private SystemsManager manager;
    #endregion


    // Use this for initialization
    void Start()
    {
        empTimer = 0f;
        Cooldown = 0f;
        Activated = false;

        shockwave = transform.GetChild(0).gameObject;
        shockwave.SetActive(Activated);

        manager = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemsManager>();
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
                Activate(false);
        }

        if (Input.GetKey(KeyCode.E))
            manager.ActivateSystem(SystemType.EMP);
    }


    #region Modifiers
    public void Activate(bool boolean)
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