using UnityEngine;

public class HyperdriveSystem : ShipSystem
{

    #region Properties

    private PlayerMovement stats;
    private GameObject m_playerMove;

    private GameObject particles;
    private Vector3 particleOriginPos;
    private ParticleSystem particlesys;
    #endregion

    // Use this for initialization
    void Start () {
        maxCooldown = 40f;

        m_playerMove = GameObject.FindGameObjectWithTag("Player");
        particles = GameObject.Find("WarpDriveParticles");

        stats = m_playerMove.GetComponent<PlayerMovement>();

        particleOriginPos = particles.transform.localPosition;
        particles.SetActive(false);
        particlesys = particles.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update() {

        if (cooldown > 0f)
            cooldown -= Time.deltaTime;


        if (Activated) 
            InitializeHyperdriveSequence();

        if (stats.MoveData.Speed < 50)
            particlesys.startSpeed = 0;
        else if (stats.MoveData.Speed >= 50 && stats.MoveData.Speed < 100)
            particlesys.startSpeed = stats.MoveData.Speed * 0.2f;
        else
            particlesys.startSpeed = ((stats.MoveData.Speed * 0.125f) - 12.5f) + 50.0f;
    }

    #region Private Methods

    public void InitializeHyperdriveSequence() {
        DeActivate();
        stats.MoveData.Boost = 5.0f;
        stats.MoveData.Acceleration = 100f;
        stats.boostActive = true;
        particles.SetActive(true);
        Invoke("RevertBoost",10f);
    }

    public void RevertBoost()
    {
        stats.MoveData.Boost = 1.0f;
        stats.MoveData.Acceleration = 20f;
        stats.boostActive = false;
        particlesys.enableEmission = false;
        Invoke("FinishParticles", 4f);
    }

    public void FinishParticles()
    {
        particlesys.enableEmission = true;
        particles.SetActive(false);
    }
    #endregion
}
