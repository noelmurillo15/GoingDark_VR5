using UnityEngine;
using System.Collections;

public class TransitionHyperDrive : MonoBehaviour {

    private bool activated;

    private float boostTimer;
    private float cooldownTimer;
    private float initializeTimer;

    private GameObject particles;
    private Vector3 particleOriginPos;

    // Use this for initialization
    void Start()
    {
        activated = false;
        cooldownTimer = 0.0f;

        if (particles == null)
            particles = GameObject.Find("WarpDriveParticles");

        particleOriginPos = particles.transform.localPosition;
        particles.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer > 0.0f)
            cooldownTimer -= Time.deltaTime;

        if (activated)
            HyperDriveBoost();
    }

    public void HyperDriveBoost()
    {
        if (initializeTimer > 0.0f)
        {
            initializeTimer -= Time.deltaTime;
            boostTimer = 0.25f;
            particles.transform.Translate(Vector3.forward * 70.0f * Time.deltaTime);
        }
        else {
            if (boostTimer > 0.0f)
            {
                boostTimer -= Time.deltaTime;
                particles.transform.Translate(Vector3.forward * 70.0f * Time.deltaTime);
               
            }
            else {
                activated = false;
                particles.transform.localPosition = particleOriginPos;
                particles.SetActive(false);
            }
        }
    }

    public void HyperDriveInitialize()
    {
        if (cooldownTimer <= 0.0f)
        {
            cooldownTimer = 15.0f;
            activated = true;
            particles.SetActive(true);
            initializeTimer = 5.0f;
        }
    }

    public float GetHyperDriveCooldown()
    {
        return cooldownTimer;
    }

    public bool IsOver()
    {
        if (initializeTimer <= 3.0f)
        {
            return true;
        }
        return false;
    }
}
