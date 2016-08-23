using UnityEngine;

public class DeathTransition : MonoBehaviour
{
    private UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration blackOut;
    private bool isDead;
    public bool spawn;
    public bool flip;

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
                blackOut.intensity = 0f;
        }
    }

    public void Death()
    {
        isDead = true;
    }

    public void SpawnPlayer()
    {
        spawn = true;
    }

    public void NotDead()
    {
        isDead = false;
    }

    public void notSpawned()
    {
        spawn = false;
    }

}
