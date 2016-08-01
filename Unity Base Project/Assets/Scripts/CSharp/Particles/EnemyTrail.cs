using UnityEngine;
using MovementEffects;
using System.Collections.Generic;

public class EnemyTrail : MonoBehaviour
{
    private Color col;
    private int numTrails;
    public HealthProperties HealthInfo;
    private TrailRenderer[] trails = new TrailRenderer[3];


    // Use this for initialization
    void Start()
    {
        Invoke("FindEnemyData", 5f);
    }

    void FindEnemyData()
    {               
        Timing.RunCoroutine(CheckHealth());
    }

    public void Kill()
    {
        Timing.KillAllCoroutines();
    }

    private IEnumerator<float> CheckHealth()
    {
        Debug.Log("Getting Components");
        trails = GetComponentsInChildren<TrailRenderer>();
        HealthInfo = GetComponent<EnemyStateManager>().GetHealthData();
        numTrails = trails.Length;
        col = Color.green;

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
                trails[x].material.SetColor("_Color", Color.green);
            }

            yield return Timing.WaitForSeconds(.5f);
        }
    }
}
