using UnityEngine;

public class DeathTransition : MonoBehaviour
{
    private UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration blackOut;
    private bool isDead;
    public bool spawn;
    bool flip;
    // Use this for initialization
    void Start()
    {
        spawn = false;
        blackOut = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead || spawn)
        {
            blackOut.intensity += Time.deltaTime;
            if (blackOut.intensity > 1f)
                blackOut.intensity = 1f;
        }
        else
        {
            blackOut.intensity -= Time.deltaTime;
            if (blackOut.intensity < 0f)
            {
                blackOut.intensity = 0f;
            }
        }

        Blackout();
    }

    void Death()
    {
        isDead = true;
    }

    void Blackout()
    {
        if (spawn && !flip)
        {
            blackOut.intensity += Time.deltaTime;
            if (blackOut.intensity > 1f)
            {
                blackOut.intensity = 1f;
                flip = true;
            }
        }
        else
        {
            blackOut.intensity -= Time.deltaTime;
            if (blackOut.intensity < 0f)
            {
                blackOut.intensity = 0f;
            }
            spawn = false;
            flip = false;
            // Invoke("DespawnPLayer",2.5f)
        }
    }

    public void SpawnPlayer()
    {
        spawn = !spawn;
    }

    public void DeSpawnPlayer()
    {
        spawn = false;
    }

    void Respawn()
    {
        isDead = false;
    }
}
