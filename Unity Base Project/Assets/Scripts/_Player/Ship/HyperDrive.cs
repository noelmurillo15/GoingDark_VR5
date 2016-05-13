using UnityEngine;
using GD.Core.Enums;

public class HyperDrive : MonoBehaviour {
    //**        Attach to HyperDrive Object     **//

    #region Properties
    public DeviceStatus Status { get; private set; }
    public bool Activated { get; private set; }
    public float Cooldown { get; private set; }

    private float boostTimer;
    private float initializeTimer;

    private GameObject particles;
    private Vector3 particleOriginPos;

    private GameObject m_playerMove;

    private PlayerStats stats;

    private float beginningTimer;
    #endregion

    // Use this for initialization
    void Start () {
        beginningTimer = 2f;
        Activated = false;
        Cooldown = 0.0f;        

        if (m_playerMove == null)
            m_playerMove = GameObject.FindGameObjectWithTag("Player");

        if (particles == null)
            particles = GameObject.Find("WarpDriveParticles");

        stats = m_playerMove.GetComponent<PlayerStats>();

        particleOriginPos = particles.transform.localPosition;
        particles.SetActive(true);
    }

    // Update is called once per frame
    void Update () {

        if (beginningTimer > 0.0f)
        {
            beginningTimer -= Time.deltaTime;
            particles.transform.Translate(Vector3.forward * 150.0f * Time.deltaTime);
            if (beginningTimer <= 0.0f)
            {
                particles.SetActive(false);
                particles.transform.localPosition = particleOriginPos;
            }
        }
        
        if (Cooldown > 0.0f)
            Cooldown -= Time.deltaTime;

        if (Activated)
            HyperDriveBoost();
    }

    #region Private Methods
    public void HyperDriveBoost() {
        if (initializeTimer > 0.0f) {
            initializeTimer -= Time.deltaTime;
            boostTimer = 0.5f;
            stats.DecreaseSpeed();
            particles.transform.Translate(Vector3.forward * 50.0f * Time.deltaTime);
        }
        else {
            if (boostTimer > 0.0f) {
                boostTimer -= Time.deltaTime;
                particles.transform.Translate(Vector3.forward * 150.0f * Time.deltaTime);
                m_playerMove.transform.Translate(Vector3.forward * 3000.0f * Time.deltaTime);
            }
            else {
                Activated = false;
                particles.transform.localPosition = particleOriginPos;
                particles.SetActive(false);
            }
        }
    }

    public void HyperDriveInitialize() {
        if (Cooldown <= 0.0f) {
            Cooldown = 15.0f;
            Activated = true;
            particles.SetActive(true);
            initializeTimer = 5.0f;
        }
    }
    #endregion
}
