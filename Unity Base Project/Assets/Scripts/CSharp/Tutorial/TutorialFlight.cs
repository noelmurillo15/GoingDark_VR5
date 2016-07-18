using UnityEngine;
using GoingDark.Core.Enums;
using UnityEngine.SceneManagement;

public class TutorialFlight : MonoBehaviour
{
    //private int phase;
    private MissionSystem mission;
    //private bool buffer, isNearStation;
    //private bool flightMissionCompleted, flightMissionAccepted, flightMissionTurnedIn;
    //private bool stealthMissionCompleted, stealthMissionAccepted, stealthMissionTurnedIn;
    //private bool combatMissionCompleted, combatMissionAccepted, combatMissionTurnedIn;
    //private bool stealthMissionFailed;
    //private GameObject flightPortal, stealthPortal;
    //private GameObject station;
    //  private BoxCollider hyperDriveButton;
    private GameObject[] loots;
   // private int numRings;
    public GameObject arrow;
    private GameObject supplyBox;
    //private GameObject stealthEnemy, combatEnemy;
    //private GameObject stationLog;
    //private AudioSource wellDone;
    //private AudioSource fantastic;
    //private AudioSource goodJob;
    //private AudioSource congratulation;
    //private EnemyStateManager[] stealthEnemies;

    // Use this for initialization
    void Start()
    {
        mission = GameObject.Find("PersistentGameObject").GetComponent<MissionSystem>();
       // station = GameObject.Find("Station");
        // hyperDriveButton = GameObject.Find("HyperdriveButton").GetComponent<BoxCollider>();
        loots = GameObject.FindGameObjectsWithTag("Loot");
        supplyBox = GameObject.Find("SupplyBox");
        //stealthEnemy = GameObject.Find("SteathEnemy");
        //stealthEnemies = stealthEnemy.transform.GetComponentsInChildren<EnemyStateManager>();
        //combatEnemy = GameObject.Find("CombatEnemy");
        //stationLog = GameObject.Find("MissionLog");
        //fantastic = transform.FindChild("Fantastic").GetComponent<AudioSource>();
        //wellDone = transform.FindChild("WellDone").GetComponent<AudioSource>();
        //goodJob = transform.FindChild("GoodJob").GetComponent<AudioSource>();
        //congratulation = transform.FindChild("Congratulation").GetComponent<AudioSource>();
        //flightPortal = GameObject.Find("Flight").transform.FindChild("PortalEnter").gameObject;
        //stealthPortal = GameObject.Find("Stealth").transform.FindChild("PortalEnter").gameObject;

        //phase = 0;
        //numRings = 0;
        //buffer = false;
        //flightMissionCompleted = false;
        //flightMissionAccepted = false;
        //flightMissionTurnedIn = false;
        //stealthMissionCompleted = false;
        //stealthMissionAccepted = false;
        //stealthMissionTurnedIn = false;
        //combatMissionCompleted = false;
        //combatMissionAccepted = false;
        //combatMissionTurnedIn = false;
        //stealthMissionFailed = false;
        //isNearStation = false;
        //flightPortal.SetActive(false);
        //stealthPortal.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Progress();
        Debug.Log(loots.Length);
        if (loots.Length > 1)
        {
            arrow.transform.LookAt(GetActiveRing());
        }
        else if (loots.Length == 1)
        {
            arrow.transform.LookAt(supplyBox.transform);
        }
        else
            arrow.SetActive(false);

        if (mission.m_PrimaryMissions.Count==0)
        {
            //LeaveScene();
        }
    }

    //void Progress()
    //{
    //    //string string1, string2, string3;
    //    switch (phase)
    //    {
    //        case 0:
    //            if (!buffer)
    //            {
    //                for (int i = 0; i < loots.Length; i++)
    //                    loots[i].SetActive(false);

    //                stealthEnemy.SetActive(false);
    //                combatEnemy.SetActive(false);
    //                buffer = true;
    //            }
    //            arrow.transform.LookAt(station.transform.position);
    //            if (isNearStation)
    //            {
    //                arrow.SetActive(false);
    //                buffer = false;
    //                phase = 1;
    //            }
    //            break;
    //        case 1:
    //            if (!buffer)
    //            {
    //                buffer = true;
    //            }
    //            if (flightMissionAccepted || stealthMissionAccepted || combatMissionAccepted)
    //            {
    //                phase = 2;
    //                buffer = false;
    //            }
    //            break;

    //        case 2:
    //            OnMissionAccept();
    //            break;

    //        case 3:
    //            OnMissionComplete();
    //            break;

    //        case 4:
    //            if (!buffer)
    //            {
    //                supplyBox.SetActive(false);
    //                stealthEnemy.SetActive(false);
    //                stealthMissionAccepted = false;
    //                stealthMissionFailed = false;
    //                for (int i = 0; i < stealthEnemies.Length; i++)
    //                {
    //                    stealthEnemies[i].SetEnemyTarget(null);
    //                }
    //                buffer = true;
    //            }
    //            arrow.transform.LookAt(station.transform);
    //            if (isNearStation)
    //            {
    //                arrow.SetActive(false);
    //                buffer = false;
    //                phase = 1;
    //            }
    //            break;
    //    }
    //}

    //    private void OnMissionAccept()
    //    {
    //        if (flightMissionAccepted)
    //        {
    //            if (!buffer)
    //            {
    //                for (int i = 0; i < loots.Length; i++)
    //                {
    //                    if (loots[i] == null)
    //                        continue;
    //                    if(loots[i].name != "SupplyBox")
    //                        loots[i].SetActive(true);
    //                }
    //                arrow.SetActive(true);
    //                buffer = true;
    //            }
    //            arrow.transform.LookAt(GetActiveRing());

    //            if (flightMissionCompleted)
    //            {
    //                phase++;
    //                buffer = false;
    //            }
    //        }
    //        else if (stealthMissionAccepted)
    //        {
    //            if (!buffer)
    //            {
    //                supplyBox.SetActive(true);
    //                buffer = true;
    //                arrow.SetActive(true);
    //                stealthEnemy.SetActive(true);
    //            }
    //            if(supplyBox)
    //                arrow.transform.LookAt(supplyBox.transform);

    //            if (stealthMissionFailed)
    //            {
    //                phase = 4;
    //                buffer = false;
    //                return;
    //            }

    //            if (stealthMissionCompleted)
    //            {
    //                phase++;
    //                buffer = false;
    //            }
    //        }
    //        else if (combatMissionAccepted)
    //        {
    //            if (!buffer)
    //            {               
    //                buffer = true;
    //                combatEnemy.SetActive(true);
    //            }

    //            if (combatMissionCompleted || mission.m_ActiveMissions[0].objectives <=0)
    //            {
    //                combatMissionCompleted = true;
    //                phase++;
    //                buffer = false;
    //            }
    //        }
    //    }
    //    private void OnMissionComplete()
    //    {
    //        if (flightMissionCompleted)
    //        {
    //            if (!buffer)
    //            {
    //                goodJob.Play();
    //                buffer = true;
    //                flightMissionAccepted = false;
    //                flightPortal.SetActive(true);
    //            }

    //            if (flightMissionTurnedIn)
    //            {
    //                buffer = false;
    //                flightMissionCompleted = false;
    //                phase = 1;
    //                arrow.SetActive(false);
    //            }
    //        }
    //        else if (stealthMissionCompleted)
    //        {
    //            if (!buffer)
    //            {
    //                wellDone.Play();
    //                buffer = true;
    //                arrow.SetActive(true);
    //                stealthMissionAccepted = false;
    //                stealthEnemy.SetActive(false);
    //                stealthPortal.SetActive(true);
    //            }

    //            if (stealthMissionTurnedIn)
    //            {
    //                buffer = false;
    //                stealthMissionCompleted = false;
    //                phase = 1;
    //                arrow.SetActive(false);
    //            }
    //        }
    //        else if (combatMissionCompleted)
    //        {
    //            if (!buffer)
    //            {
    //                fantastic.Play();
    //                buffer = true;
    //                combatMissionAccepted = false;
    //                combatEnemy.SetActive(false);
    //            }

    //            if (combatMissionTurnedIn)
    //            {
    //                buffer = false;
    //                combatMissionCompleted = false;
    //                Invoke("LeaveScene", 5.0f);
    //                phase = 1;
    //            }
    //        }
    //    }

    void LeaveScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private Vector3 GetActiveRing()
    {
        Vector3 target = Vector3.zero;
        for (int i = 0; i < loots.Length; i++)
        {
            if (!loots[i] || loots[i].name == "SupplyBox")
            {
                continue;
            }
            else if (loots[i].GetComponent<BoxCollider>().enabled)
            {
                target = loots[i].transform.position;
                return target;
            }
        }
        return target;
    }

//    void AddRingCount()
//    {
//        numRings++;
//    }

//    public void MissionCompleted(MissionType type)
//    {
//        if (type == MissionType.Scavenge)
//            flightMissionCompleted = true;
//        else if (type == MissionType.Stealth)
//            stealthMissionCompleted = true;
//        else if (type == MissionType.Combat)
//            combatMissionCompleted = true; 

//    }
//    public void MissionAccepted(MissionType type)
//    {
//        if (type == MissionType.Scavenge)
//            flightMissionAccepted = true;
//        else if (type == MissionType.Stealth)
//            stealthMissionAccepted = true;
//        else if (type == MissionType.Combat)
//            combatMissionAccepted = true;
//    }
//    public void MissionTurnedIn(MissionType type)
//    {
//        if (type == MissionType.Scavenge)
//            flightMissionTurnedIn = true;
//        else if (type == MissionType.Stealth)
//            stealthMissionTurnedIn = true;
//        else if (type == MissionType.Combat)
//            combatMissionTurnedIn = true;
//    }

//    public void EnterStation()
//    {
//        isNearStation = true;
//    }

//    public void ExitStation()
//    {
//        isNearStation = false;
//    }
}
