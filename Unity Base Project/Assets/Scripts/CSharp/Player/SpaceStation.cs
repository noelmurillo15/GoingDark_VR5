﻿using UnityEngine;

public class SpaceStation : MonoBehaviour
{
    #region Properties
    [SerializeField]
    public bool enemyTakeOver;

    public int enemyCount;

    private MessageScript msgs;

    private float repairTimer;
    private AudioSource sound;
    private SystemManager systemManager;
    #endregion


    // Use this for initialization
    void Start()
    {
        repairTimer = 0f;
        sound = GetComponent<AudioSource>();
        systemManager = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemManager>();

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

                msgs.SendMessage("StationTakeOver");
                Debug.Log("You have taken over this Station!");
            }
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.transform.tag == "Player" && repairTimer <= 0f)
        {
            sound.Play();
            repairTimer = 60f;
            systemManager.FullSystemRepair();
        }
    }
}
