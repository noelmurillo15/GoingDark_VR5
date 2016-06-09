using UnityEngine;
using System.Collections;

public class DeathTransition : MonoBehaviour {
    private UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration blackOut;
    private bool isDead;
    // Use this for initialization
    void Start () {
        blackOut = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UnityStandardAssets.ImageEffects.VignetteAndChromaticAberration>();

    }

    // Update is called once per frame
    void Update () {
        if (isDead)
        {
            blackOut.intensity += Time.deltaTime;
            if (blackOut.intensity > 1f)
                blackOut.intensity = 1f;
        }
        else
        {
            blackOut.intensity -= Time.deltaTime;
            if (blackOut.intensity <0f)
            {
                blackOut.intensity = 0f;
            }
        }

    }

    void Death()
    {
        isDead = true;
    }
    void Respawn()
    {
        isDead = false;
    }
}
