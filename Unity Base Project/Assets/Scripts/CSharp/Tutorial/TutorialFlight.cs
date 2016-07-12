﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using GoingDark.Core.Enums;

public class TutorialFlight : MonoBehaviour
{
    private int phase;
    private MissionSystem mission;
    //private Text line1, line2, line3;
    private bool buffer, isNearStation;
    private bool flightMissionCompleted, flightMissionAccepted, flightMissionTurnedIn;
    private bool stealthMissionCompleted, stealthMissionAccepted, stealthMissionTurnedIn;
    private bool combatMissionCompleted, combatMissionAccepted, combatMissionTurnedIn;
    private bool stealthMissionFailed;
    private GameObject flightPortal, stealthPortal;
    private GameObject station;
    //  private BoxCollider hyperDriveButton;
    private GameObject[] loots;
    private int numRings;
    public GameObject arrow;
    private GameObject supplyBox;
    private GameObject stealthEnemy, combatEnemy;
    private GameObject stationLog;
    private AudioSource wellDone;
    private AudioSource fantastic;
    private AudioSource goodJob;
    private AudioSource congratulation;
    private EnemyBehavior[] stealthEnemies;

    // Use this for initialization
    void Start()
    {
        mission = GameObject.Find("PersistentGameObject").GetComponent<MissionSystem>();
        station = GameObject.Find("Station");
        // hyperDriveButton = GameObject.Find("HyperdriveButton").GetComponent<BoxCollider>();
        loots = GameObject.FindGameObjectsWithTag("Loot");
        supplyBox = GameObject.Find("SupplyBox");
        stealthEnemy = GameObject.Find("SteathEnemy");
        stealthEnemies = stealthEnemy.transform.GetComponentsInChildren<EnemyBehavior>();
        combatEnemy = GameObject.Find("CombatEnemy");
        stationLog = GameObject.Find("MissionLog");
        fantastic = transform.FindChild("Fantastic").GetComponent<AudioSource>();
        wellDone = transform.FindChild("WellDone").GetComponent<AudioSource>();
        goodJob = transform.FindChild("GoodJob").GetComponent<AudioSource>();
        congratulation = transform.FindChild("Congratulation").GetComponent<AudioSource>();
        flightPortal = GameObject.Find("Flight").transform.FindChild("PortalEnter").gameObject;
        stealthPortal = GameObject.Find("Stealth").transform.FindChild("PortalEnter").gameObject;
        //line1 = GameObject.Find("Line1").GetComponent<Text>();
        //line2 = GameObject.Find("Line2").GetComponent<Text>();
        //line3 = GameObject.Find("Line3").GetComponent<Text>();
        //line1.text = "Welcome, Captain!";
        //line2.text = "";
        //line3.text = "";
        phase = 0;
        numRings = 0;
        buffer = false;
        flightMissionCompleted = false;
        flightMissionAccepted = false;
        flightMissionTurnedIn = false;
        stealthMissionCompleted = false;
        stealthMissionAccepted = false;
        stealthMissionTurnedIn = false;
        combatMissionCompleted = false;
        combatMissionAccepted = false;
        combatMissionTurnedIn = false;
        stealthMissionFailed = false;
        isNearStation = false;
        flightPortal.SetActive(false);
        stealthPortal.SetActive(false);
        //   hyperDriveButton.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {      
       // DisableStationLog();

        Progress();
    }

    void Progress()
    {
        //string string1, string2, string3;
        switch (phase)
        {
            case 0:
                if (!buffer)
                {
                    for (int i = 0; i < loots.Length; i++)
                        loots[i].SetActive(false);
                    stealthEnemy.SetActive(false);
                    combatEnemy.SetActive(false);

                    //string1 = "Push the left trigger to accelerate the ship";
                    //string2 = "Use the left stick to rotate the ship";
                    //string3 = "Drive to the station (Hint: Follow the arrow)";
                    //StartCoroutine(Delay(2.0f, string1, string2, string3));
                    buffer = true;
                }
                arrow.transform.LookAt(station.transform.position);
                if (isNearStation)
                {
                    arrow.SetActive(false);
                  //  ClearText();
                    buffer = false;
                    phase = 1;
                }
                break;
            case 1:
                if (!buffer)
                {
                    //string1 = "Clap your hands to open the Arm Menu";
                    //string2 = "Click the mission log button";
                    //string3 = "Accept one of the missions.You may only pick one at a time.";
                    //StartCoroutine(Delay(0.0f, string1, string2, string3));
                    buffer = true;
                }
                if (flightMissionAccepted || stealthMissionAccepted || combatMissionAccepted)
                {
                  //  ClearText();
                    phase = 2;
                    buffer = false;

                }
                break;

            case 2:
                OnMissionAccept();
                break;

            case 3:
                OnMissionComplete();
                break;

            case 4:
                if (!buffer)
                {
                   // ClearText();
                    supplyBox.SetActive(false);
                    stealthEnemy.SetActive(false);
                    stealthMissionAccepted = false;
                    stealthMissionFailed = false;
                    for (int i = 0; i < stealthEnemies.Length; i++)
                    {
                        stealthEnemies[i].SetEnemyTarget(null);
                    }

                    //string1 = "You Have failed the mission";
                    //string2 = "Head back to the station (Hint: Follow the arrow)";
                    //string3 = "You may try again or take on a different mission!";
                    //StartCoroutine(Delay(0.5f, string1, string2, string3));
                    buffer = true;
                }
                arrow.transform.LookAt(station.transform);
                if (isNearStation)
                {
                    arrow.SetActive(false);
                  //  ClearText();
                    buffer = false;
                    phase = 1;
                }
                break;
            default:
                break;
        }
    }

    private void OnMissionAccept()
    {
       // string string1, string2, string3;
        if (flightMissionAccepted)
        {
            if (!buffer)
            {
                for (int i = 0; i < loots.Length; i++)
                {
                    if (loots[i] == null)
                        continue;
                    if(loots[i].name != "SupplyBox")
                        loots[i].SetActive(true);
                }
                arrow.SetActive(true);
                buffer = true;
            }
            //line1.text = "Hit all the fire rings to complete the mission";
            //line2.text = "Completion (" + numRings + " / 10)";
            //line3.text = "Hint: Follow the arrow";
            arrow.transform.LookAt(GetActiveRing());

            if (flightMissionCompleted)
            {
                phase++;
                buffer = false;
            }
        }
        else if (stealthMissionAccepted)
        {
            if (!buffer)
            {
                supplyBox.SetActive(true);
                //string1 = "Use the cloak to make yourself invisible to the enemies";
                //string2 = "Obatain the supply box without being seen, or you will fail the mission";
                //string3 = "Hint: Follow the arrow";
               // StartCoroutine(Delay(0.0f, string1, string2, string3));
                buffer = true;
                arrow.SetActive(true);
                stealthEnemy.SetActive(true);
            }
            if(supplyBox)
                arrow.transform.LookAt(supplyBox.transform);

            if (stealthMissionFailed)
            {
                phase = 4;
                buffer = false;
                return;
            }

            if (stealthMissionCompleted)
            {
                phase++;
                buffer = false;
            }
        }
        else if (combatMissionAccepted)
        {
            if (!buffer)
            {               
                buffer = true;
                combatEnemy.SetActive(true);
            }
            //line1.text = "Go slay some enemies!";
            //line2.text = "Completion (" + mission.m_ActiveMissions[0].objectives + ")";
            //line3.text = "You can use Missile or Laser to eliminate the enemies";

            if (combatMissionCompleted || mission.m_ActiveMissions[0].objectives <=0)
            {
                combatMissionCompleted = true;
                phase++;
                buffer = false;
            }
        }
    }
    private void OnMissionComplete()
    {
        //string string1, string2, string3;
        if (flightMissionCompleted)
        {
            if (!buffer)
            {
                //string1 = "Good Job!";
                //string2 = "You may return to the Station and turn in the mission!";
                //string3 = "Hint: Follow the arrow";
              //  StartCoroutine(Delay(1.0f, string1, string2, string3));
                goodJob.Play();
                buffer = true;
                flightMissionAccepted = false;
                flightPortal.SetActive(true);
            }

            if (flightMissionTurnedIn)
            {
                buffer = false;
              //  ClearText();
                flightMissionCompleted = false;
                phase = 1;
                arrow.SetActive(false);
            }
        }
        else if (stealthMissionCompleted)
        {
            if (!buffer)
            {
                //string1 = "Well done!";
                //string2 = "You may return to the Station and turn in the mission!";
                //string3 = "Hint: Follow the arrow";
               // StartCoroutine(Delay(1.0f, string1, string2, string3));
                wellDone.Play();
                buffer = true;
                arrow.SetActive(true);
                stealthMissionAccepted = false;
                stealthEnemy.SetActive(false);
                stealthPortal.SetActive(true);
            }

            if (stealthMissionTurnedIn)
            {
                buffer = false;
               // ClearText();
                stealthMissionCompleted = false;
                phase = 1;
                arrow.SetActive(false);
            }
        }
        else if (combatMissionCompleted)
        {
            if (!buffer)
            {
                //string1 = "Fantastic!";
                //string2 = "You may return to the Station and turn in the mission!";
                //string3 = "Hint: Follow the arrow";
                //StartCoroutine(Delay(1.0f, string1, string2, string3));
                fantastic.Play();
                buffer = true;
                combatMissionAccepted = false;
                combatEnemy.SetActive(false);
            }

            if (combatMissionTurnedIn)
            {
                buffer = false;
                //ClearText();
                combatMissionCompleted = false;
                phase = 1;
                Invoke("LeaveScene", 2.0f);
            }
        }
    }

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

    //private void DisableStationLog()
    //{
    //    if ((combatMissionAccepted|| stealthMissionAccepted ||flightMissionAccepted) &&!stealthMissionFailed)
    //    {
    //        // stationLog.transform.FindChild("StationMissionPanel").gameObject.SetActive(false);
    //        for (int i = 0; i < stationLog.transform.FindChild("StationMissionPanel").childCount; i++)
    //        {
    //            if (stationLog.transform.FindChild("StationMissionPanel").GetChild(i))
    //            {
    //                if (stationLog.transform.FindChild("StationMissionPanel").GetChild(i).name != mission.m_ActiveMissions[0].missionName)
    //                {
    //                    stationLog.transform.FindChild("StationMissionPanel").GetChild(i).gameObject.SetActive(false);
    //                }
    //            }
    //        }
    //        //stationLog.transform.FindChild("StationPanel").gameObject.SetActive(false);
    //    }
    //    else if (((combatMissionCompleted && !combatMissionTurnedIn) || (stealthMissionCompleted && !stealthMissionTurnedIn) || (flightMissionCompleted && !flightMissionTurnedIn)))
    //    {
    //        // stationLog.transform.FindChild("StationMissionPanel").gameObject.SetActive(false);
    //        for (int i = 0; i < stationLog.transform.FindChild("StationMissionPanel").childCount; i++)
    //        {
    //            if (stationLog.transform.FindChild("StationMissionPanel").GetChild(i))
    //            {
    //                if (stationLog.transform.FindChild("StationMissionPanel").GetChild(i).name != mission.m_ActiveMissions[0].missionName)
    //                {
    //                    stationLog.transform.FindChild("StationMissionPanel").GetChild(i).gameObject.SetActive(false);
    //                }
    //            }
    //        }
    //        //stationLog.transform.FindChild("StationPanel").gameObject.SetActive(false);
    //    }
    //}

    //IEnumerator Delay(float length, string s1, string s2, string s3)
    //{
    //    yield return new WaitForSeconds(length);
    //    line1.text = s1;
    //    AudioManager.instance.PlayMessagePop();
    //    yield return new WaitForSeconds(2f);
    //    line2.text = s2;
    //    AudioManager.instance.PlayMessagePop();
    //    yield return new WaitForSeconds(2f);
    //    line3.text = s3;
    //    AudioManager.instance.PlayMessagePop();
    //}

    //IEnumerator Transition()
    //{
    //    congratulation.Play();
    //    line1.text = "Congratulations! You have completed your training!";
    //    yield return new WaitForSeconds(0.5f);
    //    line2.text = "Returning..";
    //    yield return new WaitForSeconds(0.5f);
    //    line3.text = "3..";
    //    yield return new WaitForSeconds(1f);
    //    line3.text = "2..";
    //    yield return new WaitForSeconds(1f);
    //    line3.text = "1..";
    //    yield return new WaitForSeconds(1f);
    //    SceneManager.LoadScene("MainMenu");

    //}

    //private void ClearText()
    //{
    //    line1.text = "";
    //    line2.text = "";
    //    line3.text = "";
    //}

    void AddRingCount()
    {
        numRings++;
    }

    public void MissionCompleted(MissionType type)
    {
        if (type == MissionType.Scavenge)
            flightMissionCompleted = true;
        else if (type == MissionType.Stealth)
            stealthMissionCompleted = true;
        else if (type == MissionType.Combat)
            combatMissionCompleted = true; 
        
    }
    public void MissionAccepted(MissionType type)
    {
        if (type == MissionType.Scavenge)
            flightMissionAccepted = true;
        else if (type == MissionType.Stealth)
            stealthMissionAccepted = true;
        else if (type == MissionType.Combat)
            combatMissionAccepted = true;
    }
    public void MissionTurnedIn(MissionType type)
    {
        if (type == MissionType.Scavenge)
            flightMissionTurnedIn = true;
        else if (type == MissionType.Stealth)
            stealthMissionTurnedIn = true;
        else if (type == MissionType.Combat)
            combatMissionTurnedIn = true;
    }

    public void MissionFailed(string name)
    {
        if (name == "Stealth Tutorial")
            stealthMissionFailed = true;
    }

    public void EnterStation()
    {
        isNearStation = true;
    }

    public void ExitStation()
    {
        isNearStation = false;

    }
}