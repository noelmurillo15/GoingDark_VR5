using UnityEngine;
using MovementEffects;
using System.Collections.Generic;

public class EnemyTrail : MonoBehaviour
{
    public Color col;
    public int numTrails;
    public HealthProperties HealthInfo;
    private TrailRenderer[] trails = new TrailRenderer[3];


    // Use this for initialization
    void Start()
    {
        Invoke("FindEnemyData", 3f);
    }

    void FindEnemyData()
    {               
        Timing.RunCoroutine(CheckHealth());
    }    

    private IEnumerator<float> CheckHealth()
    {
        trails = GetComponentsInChildren<TrailRenderer>();
        HealthInfo = GetComponent<EnemyStateManager>().GetHealthData();
        numTrails = trails.Length;

        while (true)
        {
            float _hp = HealthInfo.Health / HealthInfo.MaxHealth;            

            if (_hp > .75f)
                col = Color.green;            
            else if (_hp <= .75f && _hp > .25f)
                col = Color.yellow;            
            else
                col = Color.red;            

            for (int x = 0; x < numTrails; x++)
            {
                trails[x].time = _hp * 10f;
                trails[x].material.SetColor("_TintColor", col);
            }

            yield return Timing.WaitForSeconds(.25f);
        }
    }

    public void Kill()
    {
        Timing.KillAllCoroutines();
    }
}
