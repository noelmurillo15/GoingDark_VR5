using UnityEngine;

public class Spawner : MonoBehaviour {

    #region Properties
    private int count = 0;

    [SerializeField]
    private int StartCount;
    [SerializeField]
    private float countMultiplier;
    [SerializeField]
    private float startSpawnTimer;
    [SerializeField]
    private float repeatTimer;
    
    [SerializeField]
    private GameObject[] enemies;
    #endregion


    void OnEnable() {
        if (count <= 0)
            count = StartCount;

        InvokeRepeating("SpawnRandom", startSpawnTimer, repeatTimer);
    }

    private void SpawnRandom()
    {
        int num = Random.Range(0, enemies.Length);

        GameObject go = null;
        go = Instantiate(enemies[num], transform.position, Quaternion.identity) as GameObject;
        go.transform.parent = transform.parent;

        count--;
        CheckCount();   
    }

    void CheckCount()
    {
        if (count <= 0)
        {
            float newcount = StartCount * countMultiplier;
            StartCount = (int)newcount;

            CancelInvoke("SpawnRandom");
            gameObject.SetActive(false);
        }
    }
}