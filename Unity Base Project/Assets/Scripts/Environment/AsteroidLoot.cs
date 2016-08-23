using UnityEngine;

public class AsteroidLoot : MonoBehaviour {

    private MissionSystem mission;
    private PlayerStats playerStats;
    private Transform myTransform;

    // Use this for initialization
    void Start()
    {
        myTransform = transform;
        mission = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MissionSystem>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    void FixedUpdate()
    {
        myTransform.LookAt(playerStats.transform);
        myTransform.position += myTransform.forward * 200 * Time.fixedDeltaTime;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.CompareTag("Player"))
        {
            mission.AsteroidLoot();
            playerStats.UpdateCredits(100);
            AudioManager.instance.PlayCollect();
            Destroy(gameObject);
        }
    }
}
