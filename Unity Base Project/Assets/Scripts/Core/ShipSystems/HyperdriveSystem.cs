using UnityEngine;

public class HyperdriveSystem : ShipDevice
{

    #region Properties
    private bool hypeJump;
    private float beginningTimer;

    private float boostTimer;
    private float initializeTimer;

    private PlayerMovement stats;
    private GameObject m_playerMove;

    private GameObject particles;
    private Vector3 particleOriginPos;
    #endregion

    // Use this for initialization
    void Start () {
        hypeJump = false;
        maxCooldown = 60f;
        beginningTimer = 2f;    

        m_playerMove = GameObject.FindGameObjectWithTag("Player");
        particles = GameObject.Find("WarpDriveParticles");

        stats = m_playerMove.GetComponent<PlayerMovement>();

        particleOriginPos = particles.transform.localPosition;
        particles.SetActive(true);
    }

    // Update is called once per frame
    void Update() {

        if (beginningTimer > 0.0f)
        {
            beginningTimer -= Time.deltaTime;
            particles.transform.Translate(Vector3.forward * 100.0f * Time.deltaTime);
            if (beginningTimer <= 0.0f)
            {
                particles.SetActive(false);
                particles.transform.localPosition = particleOriginPos;
            }
        }        

        if (Input.GetAxisRaw("LBumper") > 0f)
        {
            if (!hypeJump)
            {
                Activate();
                return;
            }

            cooldown = 5f;
            hypeJump = false;
            boostTimer = 0f;
            beginningTimer = 0f;
            initializeTimer = 0f;
            particles.transform.localPosition = particleOriginPos;
            particles.SetActive(false);
        }

        if (Activated)
            InitializeHyperdriveSequence();

        if (hypeJump)
            HyperDriveBoost();
    }

    #region Private Methods
    public void HyperDriveBoost() {
        if (initializeTimer > 0.0f) {
            particles.SetActive(true);
            initializeTimer -= Time.deltaTime;
            boostTimer = 0.5f;
            stats.GetMoveData().DecreaseSpeed();
            particles.transform.Translate(Vector3.forward * 25.0f * Time.deltaTime);
        }
        else {
            if (boostTimer > 0.0f) {
                AudioManager.instance.PlayHyperDrive();
                boostTimer -= Time.deltaTime;
                particles.transform.Translate(Vector3.forward * 150.0f * Time.deltaTime);
                m_playerMove.transform.Translate(Vector3.forward * 8000.0f * Time.deltaTime);
            }
            else {
                particles.transform.localPosition = particleOriginPos;
                particles.SetActive(false);
                hypeJump = false;
            }
        }
    }

    public void InitializeHyperdriveSequence() {
        hypeJump = true;
        DeActivate();
        initializeTimer = 5.0f;
    }
    #endregion
}
