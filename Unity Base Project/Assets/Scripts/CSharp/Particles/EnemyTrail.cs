using UnityEngine;
using MovementEffects;
using System.Collections.Generic;

public class EnemyTrail : MonoBehaviour
{
    public HealthProperties HealthInfo;
    private TrailRenderer[] trails = new TrailRenderer[3];


    // Use this for initialization
    void Start()
    {
        Invoke("FindEnemyData", 3f);
    }

    void FindEnemyData()
    {
        trails = GetComponentsInChildren<TrailRenderer>();
        HealthInfo = GetComponent<EnemyStateManager>().GetHealthData();
        Timing.RunCoroutine(CheckHealth());
    }

    private IEnumerator<float> CheckHealth()
    {
        while (true)
        {
            float _hp = HealthInfo.Health / HealthInfo.MaxHealth;
            trails[0].time = trails[1].time = trails[2].time = (_hp * 10f);

            if (_hp > .75f)
            {
                trails[0].material.SetColor("_TintColor", Color.green);
                trails[1].material.SetColor("_TintColor", Color.green);
                trails[2].material.SetColor("_TintColor", Color.green);
            }
            else if (_hp <= .75f && _hp > .25f)
            {
                trails[0].material.SetColor("_TintColor", Color.yellow);
                trails[1].material.SetColor("_TintColor", Color.yellow);
                trails[2].material.SetColor("_TintColor", Color.yellow);
            }
            else
            {
                trails[0].material.SetColor("_TintColor", Color.red);
                trails[1].material.SetColor("_TintColor", Color.red);
                trails[2].material.SetColor("_TintColor", Color.red);
            }

            yield return Timing.WaitForSeconds(1f);
        }
    }
}
