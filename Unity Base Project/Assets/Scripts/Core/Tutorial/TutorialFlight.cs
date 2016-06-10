using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using GoingDark.Core.Enums;

public class TutorialFlight: MonoBehaviour
{

    private int phase;
    private PlayerMovement playerMovement;
    private MissionSystem mission;
    private Text line1, line2, line3;
    private bool buffer, missionCompleted, missionAccepted, missionTurnedIn, isNearStation;
    private GameObject station;
  //  private BoxCollider hyperDriveButton;
    private GameObject[] loots;
    private AudioSource tutorialRemind;
    float remindTimer;
    int numRings;
    public GameObject arrow;

    // Use this for initialization
    void Start()
    {
        mission = GameObject.Find("PersistentGameObject").GetComponent<MissionSystem>();
        playerMovement = GameObject.Find("TutorialPlayerF").GetComponent<PlayerMovement>();
        station = GameObject.Find("Station");
       // hyperDriveButton = GameObject.Find("HyperdriveButton").GetComponent<BoxCollider>();
        loots = GameObject.FindGameObjectsWithTag("Loot");
        tutorialRemind = GetComponent<AudioSource>();

        line1 = GameObject.Find("Line1").GetComponent<Text>();
        line2 = GameObject.Find("Line2").GetComponent<Text>();
        line3 = GameObject.Find("Line3").GetComponent<Text>();
        line1.text = "Welcome, Captain!";
        line2.text = "";
        line3.text = "";
        phase = 0;
        numRings = 0;
        remindTimer = 6.0f;
        buffer = false;
        missionAccepted = false;
        missionCompleted = false;
        missionTurnedIn = false;
        isNearStation = false;
        //   hyperDriveButton.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        Progress();
    }

    void Progress()
    {
        string string1, string2, string3;
        switch (phase)
        {
            case 0:
                if (!buffer)
                {
                    for (int i = 0; i < loots.Length; i++)
                    {
                        loots[i].SetActive(false);
                    }
                    string1 = "Push the left trigger to accelerate the ship";
                    string2 = "Use the left stick to rotate the ship";
                    string3 = "Drive to the station (Hint: Follow the arrow)";
                    StartCoroutine(Delay(2.0f, string1, string2, string3));
                    buffer = true;
                }
                TutorialRemind();
                arrow.transform.LookAt(station.transform);

                if (isNearStation)
                {
                    arrow.SetActive(false);
                    ClearText();
                    playerMovement.enabled = false;
                    playerMovement.StopMovement();
                    buffer = false;
                    phase++;
                    StopTutorialRemind();
                }
                break;
            case 1:
                if (!buffer)
                {
                    string1 = "Clap your hands to open the Arm Menu";
                    string2 = "Click the mission log button";
                    string3 = "Accept the first mission : " + mission.m_stationMissions[0].missionName;
                    StartCoroutine(Delay(0.0f, string1, string2, string3));
                    buffer = true;
                }
                TutorialRemind();
                if (missionAccepted)
                {
                    playerMovement.enabled = true;
                    ClearText();
                    phase++;
                    buffer = false;
                    for (int i = 0; i < loots.Length; i++)
                    {
                        loots[i].SetActive(true);
                        arrow.SetActive(true);

                    }
                    StopTutorialRemind();

                }
                break;
            
            case 2:
                line1.text = "Hit all the fire rings to complete the mission";
                line2.text = "Completion (" + numRings + " / 10)";
                line3.text = "Hint: Follow the arrow";
                arrow.transform.LookAt(GetActiveRing());
                if (numRings == 10)
                {
                    phase++;
                    //ClearText();
                }
                break;

            case 3:
                if (!buffer)
                {
                    string1 = "Congratulations!";
                    string2 = "You may return to the Station and turn in the mission!";
                    string3 = "Hint: Follow the arrow";
                    StartCoroutine(Delay(1.0f, string1, string2, string3));
                    buffer = true;
                }
                TutorialRemind();
                arrow.transform.LookAt(station.transform);

                if (isNearStation)
                {
                    playerMovement.enabled = false;
                    playerMovement.StopMovement();
                }
                if (missionTurnedIn)
                {
                    buffer = false;
                    ClearText();
                    phase++;
                    StopTutorialRemind();
                    StartCoroutine(Transition());
                }
                break;
        
            default:
                break;
        }
    }

    private Vector3 GetActiveRing()
    {
        Vector3 target = Vector3.zero;
        for (int i = 0; i < loots.Length; i++)
        {
            if (!loots[i])
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

    IEnumerator Delay(float length, string s1, string s2, string s3)
    {
        yield return new WaitForSeconds(length);
        line1.text = s1;
        //AudioManager.instance.PlayMessagePop();
        yield return new WaitForSeconds(2f);
        line2.text = s2;
        //AudioManager.instance.PlayMessagePop();
        yield return new WaitForSeconds(2f);
        line3.text = s3;
        //AudioManager.instance.PlayMessagePop();
    }

    IEnumerator Transition()
    {
        line1.text = "You have completed your training!";
        yield return new WaitForSeconds(0.5f);
        line2.text = "Returning..";
        yield return new WaitForSeconds(0.5f);
        line3.text = "3..";
        yield return new WaitForSeconds(1f);
        line3.text = "2..";
        yield return new WaitForSeconds(1f);
        line3.text = "1..";
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MainMenu");

    }

    private void ClearText()
    {
        line1.text = "";
        line2.text = "";
        line3.text = "";
    }

    void AddRingCount()
    {
        numRings++;
    }

    private void TutorialRemind()
    {
        remindTimer += Time.deltaTime;
        if (remindTimer>=12f)
        {
            tutorialRemind.Play();
            remindTimer = 0.0f;
        }
    }
    private void StopTutorialRemind()
    {
        tutorialRemind.Stop();
        remindTimer = 6f;
    }

    public void MissionCompleted(string name)
    {
        if(name == "Scavenger")
            missionCompleted = true;
    }
    public void MissionAccepted(string name)
    {
        if(name == "Scavenger")
            missionAccepted = true;
    }

    public void MissionTurnedIn(string name)
    {
        if(name == "Scavenger")
            missionTurnedIn = true;
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
