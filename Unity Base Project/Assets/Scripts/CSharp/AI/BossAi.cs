using UnityEngine;
using GoingDark.Core.Enums;

[RequireComponent(typeof(EnemyStateManager))]
public class BossAi : MonoBehaviour
{
    #region Properties
    private EnemyStateManager behavior;
    private GameObject droids;
    private ObjectPooling pool;
    private bool droidSpawned;
    private int droidcount;
    private int Maxcount;
    private bool foundP;
    #endregion

    void Start()
    {
        droidSpawned = false;
        behavior = GetComponent<EnemyStateManager>();
        droids = Resources.Load<GameObject>("Droid");
        //pool = new ObjectPooling();
        //pool.Initialize(droids, 10);

        InvokeRepeating("SpawnDroids", 1f, 4);
        droidcount = 1;
        Maxcount = -100;
        foundP = false;
    }
    

    private void SpawnDroids()
    {
        if(!foundP)
        {
            foundP = true;
            AudioManager.instance.PlayBossTheme();
        }
        GameObject[] go = new GameObject[droidcount];
        if (behavior.Target != null && Maxcount < 50)
            for (int i = 0; i < droidcount; i++)
            {
                go[i] = Instantiate(droids, transform.position, Quaternion.identity) as GameObject;
                go[i].transform.name = "Droid";
                go[i].SendMessage("LoadEnemyData");
                go[i].transform.position = behavior.transform.position;
                go[i].transform.parent = behavior.transform.parent;
                Maxcount++;
                go[i].SendMessage("SelfDestructBoss");
            }
    }
}
