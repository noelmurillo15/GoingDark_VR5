using UnityEngine;

public class KamikazeAI : MonoBehaviour
{
    #region Properties
    private float detectionTimer;
    private float selfdestructTimer;
    private EnemyStateManager behavior;
    #endregion


    // Use this for initialization
    void Start()
    {
        detectionTimer = 0f;
        selfdestructTimer = 20f;
        behavior = GetComponent<EnemyStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (detectionTimer > 0f)
            detectionTimer -= Time.deltaTime;
    }

    #region Collision Detection
    void OnCollisionEnter(Collision hit)
    {
        if (hit.transform.CompareTag("Player") && detectionTimer <= 0f)
        {
            detectionTimer = 5f;
            if (Random.Range(0, 2) == 1)
                hit.transform.SendMessage("EMPHit");
            else
                hit.transform.SendMessage("Hit");

            behavior.Kill();
        }
    }
    #endregion
}