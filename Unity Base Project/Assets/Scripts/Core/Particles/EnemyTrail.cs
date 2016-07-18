using UnityEngine;
using System.Collections;


public class EnemyTrail : MonoBehaviour
{
    public float Health;
    private int difficulty;
    private TrailRenderer[] trails = new TrailRenderer[3];
    // Use this for initialization
    void Start()
    {
        //Health = GetComponent<EnemyBehavior>().GetHealthData().Health;
        Health = 0;
        trails = GetComponentsInChildren<TrailRenderer>();
        difficulty = (int)GetComponentInParent<Transform>().GetComponentInParent<EnemyManager>().Difficulty;
        Invoke("CheckHealth", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        // 5 -> 100(300)
        switch (difficulty)
        {
            case 0:
                trails[0].time = trails[1].time = trails[2].time = ((Health / 15f) * 20f);
                if (Health > 4f && Health < 9f)
                {
                    trails[0].material.SetColor("_TintColor", Color.yellow);
                    trails[1].material.SetColor("_TintColor", Color.yellow);
                    trails[2].material.SetColor("_TintColor", Color.yellow);
                }
                else if (Health < 4f)
                {
                    trails[0].material.SetColor("_TintColor", Color.red);
                    trails[1].material.SetColor("_TintColor", Color.red);
                    trails[2].material.SetColor("_TintColor", Color.red);
                }
                else
                {
                    trails[0].material.SetColor("_TintColor", Color.green);
                    trails[1].material.SetColor("_TintColor", Color.green);
                    trails[2].material.SetColor("_TintColor", Color.green);
                }
                break;
            case 1:
                trails[0].time = trails[1].time = trails[2].time = ((Health / 30f) * 20f);
                if (Health > 12f && Health < 20f)
                {
                    trails[0].material.SetColor("_TintColor", Color.yellow);
                    trails[1].material.SetColor("_TintColor", Color.yellow);
                    trails[2].material.SetColor("_TintColor", Color.yellow);
                }
                else if (Health < 12f)
                {
                    trails[0].material.SetColor("_TintColor", Color.red);
                    trails[1].material.SetColor("_TintColor", Color.red);
                    trails[2].material.SetColor("_TintColor", Color.red);
                }
                else
                {
                    trails[0].material.SetColor("_TintColor", Color.green);
                    trails[1].material.SetColor("_TintColor", Color.green);
                    trails[2].material.SetColor("_TintColor", Color.green);
                }
                break;
            case 2:
                trails[0].time = trails[1].time = trails[2].time = ((Health / 50f) * 20f);
                if (Health > 15 && Health < 30f)
                {
                    trails[0].material.SetColor("_TintColor", Color.yellow);
                    trails[1].material.SetColor("_TintColor", Color.yellow);
                    trails[2].material.SetColor("_TintColor", Color.yellow);
                }
                else if (Health < 15f)
                {
                    trails[0].material.SetColor("_TintColor", Color.red);
                    trails[1].material.SetColor("_TintColor", Color.red);
                    trails[2].material.SetColor("_TintColor", Color.red);
                }
                else
                {
                    trails[0].material.SetColor("_TintColor", Color.green);
                    trails[1].material.SetColor("_TintColor", Color.green);
                    trails[2].material.SetColor("_TintColor", Color.green);
                }
                break;
            case 3:
                trails[0].time = trails[1].time = trails[2].time = ((Health / 100f) * 20f);
                if (Health > 35 && Health < 70f)
                {
                    trails[0].material.SetColor("_TintColor", Color.yellow);
                    trails[1].material.SetColor("_TintColor", Color.yellow);
                    trails[2].material.SetColor("_TintColor", Color.yellow);
                }
                else if (Health < 35f)
                {
                    trails[0].material.SetColor("_TintColor", Color.red);
                    trails[1].material.SetColor("_TintColor", Color.red);
                    trails[2].material.SetColor("_TintColor", Color.red);
                }
                else
                {
                    trails[0].material.SetColor("_TintColor", Color.green);
                    trails[1].material.SetColor("_TintColor", Color.green);
                    trails[2].material.SetColor("_TintColor", Color.green);
                }
                break;

        }
    }

    void CheckHealth()
    {
        Health = GetComponent<EnemyStateManager>().GetHealthData().Health;
    }
}
