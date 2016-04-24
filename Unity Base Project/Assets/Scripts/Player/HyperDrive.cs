using UnityEngine;
using System.Collections;

public class HyperDrive : MonoBehaviour {
    //**        Attach to HyperDrive Object     **//

    private bool activated;
    private float boostTimer;
    public float cooldownTimer;
    private float initializeTimer;

    private GameObject particles;
    private GameObject m_playerMove;
    private Vector3 particleOriginPos;


    // Use this for initialization
    void Start () {
        activated = false;
        cooldownTimer = 0.0f;        

        if (m_playerMove == null)
            m_playerMove = GameObject.FindGameObjectWithTag("Player");

        if (particles == null)
            particles = GameObject.Find("WarpDriveParticles");

        particleOriginPos = particles.transform.localPosition;
        particles.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        if (cooldownTimer > 0.0f)
            cooldownTimer -= Time.deltaTime;

        if (activated)
            HyperDriveBoost();
    }

    public void HyperDriveBoost() {
        if (initializeTimer > 0.0f) {
            initializeTimer -= Time.deltaTime;
            boostTimer = 1.0f;
            m_playerMove.GetComponent<JoyStickMovement>().StopMovement();
            particles.transform.Translate(Vector3.forward * 65.0f * Time.deltaTime);
        }
        else {
            if (boostTimer > 0.0f) {
                boostTimer -= Time.deltaTime;
                m_playerMove.transform.Translate(Vector3.forward * 400.0f * Time.deltaTime);
            }
            else {
                activated = false;
                particles.transform.localPosition = particleOriginPos;
                particles.SetActive(false);
            }
        }
    }

    public void HyperDriveInitialize() {
        if (cooldownTimer <= 0.0f) {
            cooldownTimer = 120.0f;
            activated = true;
            particles.SetActive(true);
            initializeTimer = 5.0f;
        }
    }

    public float GetHyperDriveCooldown() {
        return cooldownTimer;
    }
}
