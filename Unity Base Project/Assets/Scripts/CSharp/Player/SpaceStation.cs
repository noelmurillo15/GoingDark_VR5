using UnityEngine;

public class SpaceStation : MonoBehaviour
{
    #region Properties
    [SerializeField]
    public bool enemyTakeOver;

    public int enemyCount;

    private MessageScript msgs;

    private float repairTimer;
    private AudioSource sound;
    private PlayerStats stats;
    private MissionSystem missionSystem;
    #endregion


    // Use this for initialization
    void Start()
    {
        repairTimer = 60f;
        sound = GetComponent<AudioSource>();
        missionSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MissionSystem>();

        if (enemyTakeOver)
        {
            GameObject go = Instantiate(Resources.Load<GameObject>("EnemyStationGlow")) as GameObject;
            Vector3 loc = go.transform.position;
            go.transform.parent = transform;
            go.transform.localPosition = loc;

            enemyCount = transform.childCount;
            msgs = GameObject.Find("PlayerCanvas").GetComponent<MessageScript>();
        }
        else
        {
            GameObject go = Instantiate(Resources.Load<GameObject>("StationGlow")) as GameObject;
            Vector3 loc = go.transform.position;
            go.transform.parent = transform;
            go.transform.localPosition = loc;

        }
            stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (repairTimer > 0)
            repairTimer -= Time.deltaTime;

        if(enemyTakeOver)
        {
            enemyCount = transform.childCount;
            if(enemyCount == 1)
            {
                Destroy(transform.GetChild(0).gameObject);
                GameObject go = Instantiate(Resources.Load<GameObject>("StationGlow")) as GameObject;
                Vector3 loc = go.transform.position;
                go.transform.parent = transform;
                go.transform.localPosition = loc;
                enemyTakeOver = false;
                enemyCount = 0;


                msgs.SendMessage("StationTakeOver");                missionSystem.ControlPointTaken();
            }
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (repairTimer <= 0f)
        {
            if (col.transform.tag == "Player" && !enemyTakeOver)
            {
                sound.Play();
                stats.Repair(50);
                repairTimer = 60f;
            }
        }
    }
}
