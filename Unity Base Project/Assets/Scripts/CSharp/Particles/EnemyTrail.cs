using UnityEngine;
using GoingDark.Core.Enums;


public class EnemyTrail : MonoBehaviour
{
    public EnemyStateManager HealthInfo;
    private GameDifficulty difficulty;
    private TrailRenderer[] trails = new TrailRenderer[3];
    // Use this for initialization
    void Start()
    {
        HealthInfo = GetComponent<EnemyStateManager>();
        trails = GetComponentsInChildren<TrailRenderer>();
        difficulty = transform.root.GetComponent<EnemyManager>().Difficulty;
        Invoke("CheckHealth", 2f);
    }

    void CheckHealth()
    {
        float _hp = HealthInfo.GetHealthData().Health;
        switch (difficulty)
        {
            case GameDifficulty.Easy:
                trails[0].time = trails[1].time = trails[2].time = ((_hp / 15f) * 20f);
                if (_hp > 4f && _hp < 9f)
                {
                    trails[0].material.SetColor("_TintColor", Color.yellow);
                    trails[1].material.SetColor("_TintColor", Color.yellow);
                    trails[2].material.SetColor("_TintColor", Color.yellow);
                }
                else if (_hp < 4f)
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
            case GameDifficulty.Normal:
                trails[0].time = trails[1].time = trails[2].time = ((_hp / 30f) * 20f);
                if (_hp > 12f && _hp < 20f)
                {
                    trails[0].material.SetColor("_TintColor", Color.yellow);
                    trails[1].material.SetColor("_TintColor", Color.yellow);
                    trails[2].material.SetColor("_TintColor", Color.yellow);
                }
                else if (_hp < 12f)
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
            case GameDifficulty.Hard:
                trails[0].time = trails[1].time = trails[2].time = ((_hp / 50f) * 20f);
                if (_hp > 15 && _hp < 30f)
                {
                    trails[0].material.SetColor("_TintColor", Color.yellow);
                    trails[1].material.SetColor("_TintColor", Color.yellow);
                    trails[2].material.SetColor("_TintColor", Color.yellow);
                }
                else if (_hp < 15f)
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
            case GameDifficulty.Nightmare:
                trails[0].time = trails[1].time = trails[2].time = ((_hp / 100f) * 20f);
                if (_hp > 35 && _hp < 70f)
                {
                    trails[0].material.SetColor("_TintColor", Color.yellow);
                    trails[1].material.SetColor("_TintColor", Color.yellow);
                    trails[2].material.SetColor("_TintColor", Color.yellow);
                }
                else if (_hp < 35f)
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
}
