using UnityEngine;

public class HyperdriveSystem : ShipSystem
{

    #region Properties
    private PlayerMovement stats;
    private GameObject particles;
        #endregion

    // Use this for initialization
    void Start () {
        maxCooldown = 40f;

        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        particles = transform.GetChild(0).gameObject;
        particles.SetActive(false);
    }

    // Update is called once per frame
    void Update() {

        if (cooldown > 0f)
            cooldown -= Time.deltaTime;

        if (Activated) 
            InitializeHyperdriveSequence();
    }

    #region Private Methods
    public void InitializeHyperdriveSequence() {
        DeActivate();
        stats.GetMoveData().SetMaxSpeed(500f);
        stats.GetMoveData().SetAccel(100f);
        stats.boostActive = true;
        particles.SetActive(true);
        Invoke("StopHyperdrive", 10f);
    }

    public void StopHyperdrive()
    {
        if(IsInvoking("StopHyperdrive"))
            CancelInvoke("StopHyperdrive");

        stats.GetMoveData().SetMaxSpeed(100f);
        stats.GetMoveData().SetAccel(20f);
        stats.boostActive = false;
        particles.SetActive(false);
    }
    #endregion
}
