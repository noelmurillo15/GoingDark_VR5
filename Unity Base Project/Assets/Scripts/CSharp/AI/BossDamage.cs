using UnityEngine;
using MovementEffects;
using System.Collections.Generic;

public class BossDamage : MonoBehaviour
{
    [SerializeField]
    private EnemyStateManager st;
    private HealthProperties hp;
    [SerializeField]
    private GameObject []burns;
    
    void Start()
    {
        burns[0].SetActive(false);
        burns[1].SetActive(false);
        burns[2].SetActive(false);
        Invoke("FindBossData", 3f);
    }

    void FindBossData()
    {
        hp = st.GetHealthData();
    }

    void MissileHit(Missile missile)
    {
        float lop = (hp.Health / hp.MaxHealth);
        if (lop < .75f)
            if (!burns[0].activeSelf)
                burns[0].SetActive(true);

        if(lop < .50f)
            if (!burns[1].activeSelf)
                burns[1].SetActive(true);

        if (lop < .25f)
            if (!burns[2].activeSelf)
                burns[2].SetActive(true);
    }
    void LaserHit(LaserProjectile laser)
    {
        float lop = (hp.Health / hp.MaxHealth);
        if (lop < .75f)
            if (!burns[0].activeSelf)
                burns[0].SetActive(true);

        if (lop < .50f)
            if (!burns[1].activeSelf)
                burns[1].SetActive(true);

        if (lop < .25f)
            if (!burns[2].activeSelf)
                burns[2].SetActive(true);

    }
    void SplashDmg()
    {
        float lop = (hp.Health / hp.MaxHealth);
        if (lop < .75f)
            if (!burns[0].activeSelf)
                burns[0].SetActive(true);

        if (lop < .50f)
            if (!burns[1].activeSelf)
                burns[1].SetActive(true);

        if (lop < .25f)
            if (!burns[2].activeSelf)
                burns[2].SetActive(true);
    }
}

