using UnityEngine;
using MovementEffects;
using System.Collections.Generic;

public class EnemyTrail : MonoBehaviour
{
    public int numTrails;
    private TrailRenderer[] trails;
    private HealthProperties HealthInfo;


    // Use this for initialization
    void Start()
    {
        Invoke("FindEnemyData", 3f);
    }

    void FindEnemyData()
    {
        trails = GetComponentsInChildren<TrailRenderer>();        
        HealthInfo = GetComponent<EnemyStateManager>().GetHealthData();
        Timing.RunCoroutine(CheckHealth(HealthInfo));
    }    

    private IEnumerator<float> CheckHealth(HealthProperties _health)
    {       
        while (true)
        {
            int col = 0;
            float _hp = HealthInfo.Health / HealthInfo.MaxHealth;            

            if (_hp > .75f)
                col = 0;            
            else if (_hp <= .75f && _hp > .25f)
                col = 1;            
            else
                col = 2;

            if (trails[col] != null)
            {
                for (int x = 0; x < numTrails; x++)
                {
                    trails[x].time = _hp * 25f;
                    if (x == col)
                        trails[x].gameObject.SetActive(true);
                    else
                        trails[x].gameObject.SetActive(false);
                }
            }

            yield return Timing.WaitForSeconds(.2f);
        }
    }

    public void Kill()
    {
        Timing.KillCoroutines(CheckHealth(HealthInfo));
        Destroy(gameObject);
    }
}
