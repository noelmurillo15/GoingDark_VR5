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
            switch(difficulty)
            {
                case 0:
                    trails[0].time = trails[1].time = trails[2].time = ((Health / 15f) * 20f);
                    break;
                case 3:
                    trails[0].time = trails[1].time = trails[2].time = ((Health / 100f) * 20f);
                    break;

            }
    }

    void CheckHealth()
    {
        Health = GetComponent<EnemyBehavior>().GetHealthData().Health;
    }
}
