using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using GD.Core.Enums;

public class Tutorial : MonoBehaviour
{

    private int phase;
    private PlayerMovement playerMovement;
    private MissionSystem mission;
    private Text line1, line2, line3;
    private int deviceCollected;
    private SystemType type;
    private PlayerStats player;
    public GameObject Arrow;
    private bool buffer, missionCompleted, missionAccepted, missionTurnedIn, isNearStation;
    private GameObject[] shipParts;
    private GameObject enemy1 = null, droidBot = null;
    private SystemManager systemManager;
    private GameObject station;
    private HyperdriveSystem hyperDrive;
    private GameObject hyperDriveParticle;

    // Use this for initialization
    void Start()
    {
        mission = GameObject.Find("PersistentGameObject").GetComponent<MissionSystem>();
        player = GameObject.Find("Player").GetComponent<PlayerStats>();
        shipParts = GameObject.FindGameObjectsWithTag("Loot");
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        systemManager = GameObject.Find("Devices").GetComponent<SystemManager>();
        station = GameObject.Find("Station");
        hyperDrive = GameObject.Find("HyperDrive(Clone)").GetComponent<HyperdriveSystem>();
        hyperDriveParticle = GameObject.Find("WarpDriveParticles");

        line1 = GameObject.Find("Line1").GetComponent<Text>();
        line2 = GameObject.Find("Line2").GetComponent<Text>();
        line3 = GameObject.Find("Line3").GetComponent<Text>();
        line1.text = "Welcome, Captain!";
        line2.text = "";
        line3.text = "";
        phase = 0;
        deviceCollected = 0;
        buffer = false;
        missionAccepted = false;
        missionCompleted = false;
        missionTurnedIn = false;
        Arrow.SetActive(false);
        type = SystemType.NONE;
        isNearStation = false;
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
                    string1 = "Push the left trigger to accelerate the ship";
                    string2 = "Use the left stick to rotate the ship";
                    string3 = "Drive to the station (Hint: Follow the arrow)";
                    StartCoroutine(Delay(2.0f, string1, string2, string3));
                    buffer = true;
                    Arrow.SetActive(true);
                }
                Arrow.transform.LookAt(station.transform);
                if (!hyperDriveParticle.activeSelf)
                {
                    hyperDrive.enabled = false;
                }

                if (isNearStation)
                {
                    ClearText();
                    playerMovement.enabled = false;
                    player.StopMovement();
                    buffer = false;
                    phase++;
                }
                break;
            case 1:
                if (!buffer)
                {
                    string1 = "Touch your left forearm with your right palm to open the Arm Menu";
                    string2 = "Click the mission log button";
                    string3 = "Accept the first mission : " + mission.m_stationMissions[0].missionName;
                    StartCoroutine(Delay(0.0f, string1, string2, string3));
                    buffer = true;
                }
                                
                if (missionAccepted)
                {
                    playerMovement.enabled = true;
                    ClearText();
                    phase++;
                    buffer = false;
                }
                break;
            
            case 2:
                if (type != SystemType.NONE)
                {
                    playerMovement.enabled = false;
                    ShowDevice();
                }
                else
                {
                    line1.text = "Collect 6 ship parts to form a complete ship";
                    line2.text = "Completion (" + deviceCollected + " / 6)";
                    line3.text = "Hint: Follow the arrow";
                    Arrow.transform.LookAt(GetClosestShipPart());

                    if (missionCompleted)
                    {
                        ClearText();
                        phase++;
                    }
                }
                break;
            case 3:
                if (!buffer)
                {
                    string1 = "Congradulations!";
                    string2 = "You may return to the Station and turn in the mission!";
                    string3 = "Hint: Follow the arrow";
                    StartCoroutine(Delay(3.0f, string1, string2, string3));
                    buffer = true;
                }
                if (isNearStation)
                {
                    player.StopMovement();
                }
                    Arrow.transform.LookAt(station.transform);
                if (missionTurnedIn)
                {
                    hyperDrive.enabled = true;
                    buffer = false;
                    ClearText();
                    phase++;
                }
                break;
            case 4:
                if (!buffer)
                {
                    string1 = "You have completed your training!";
                    string2 = "Use the hyper drive to exit the scene!";
                    string3 = "Hyper drive allows you to travel with speed of light";
                    StartCoroutine(Delay(0.0f, string1, string2, string3));
                    buffer = true;
                }
                if (systemManager.GetActive(SystemType.HYPERDRIVE))
                {
                    ClearText();
                    buffer = false;
                    phase++;
                }
                break;

            case 5:
                if (!buffer)
                {
                    StartCoroutine(Transition());
                    buffer = true;
                }
                break;

            default:
                break;
        }
    }

    IEnumerator Delay(float length, string s1, string s2, string s3)
    {
        yield return new WaitForSeconds(length);
        line1.text = s1;
        yield return new WaitForSeconds(2f);
        line2.text = s2;
        yield return new WaitForSeconds(2f);
        line3.text = s3;
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

    void ShowDevice()
    {
        string s1, s2, s3;
        Debug.Log(type);
        switch (type)
        {
            case SystemType.EMP:
                if (!buffer)
                {
                    Vector3 temp = player.transform.position + player.transform.forward * 50;
                    droidBot = Instantiate(Resources.Load("Tutorial/Droid"), temp, Quaternion.identity) as GameObject;
                    droidBot.transform.parent = GameObject.Find("Enemy").transform;

                    ClearText();
                    s1 = "You now have an EMP which slows all enemies nearby";
                    s2 = "EMP is also extremely effective against droids.";
                    s3 = "Try to use it on the droid!";
                    buffer = true;
                    StartCoroutine(Delay(0.0f, s1, s2, s3));
                    player.StopMovement();
                }
                if (!droidBot)
                {
                    Vector3 temp = player.transform.position + player.transform.forward * 50;
                    droidBot = Instantiate(Resources.Load("Tutorial/Droid"), temp, Quaternion.identity) as GameObject;
                    droidBot.transform.parent = GameObject.Find("Enemy").transform;
                }
                if (systemManager.GetActive(SystemType.EMP) && droidBot)
                {
                    //Invoke("DestroyEnemy", 1f);
                    //Destroy(droidBot);
                    droidBot.GetComponent<TutorialEnemy>().Kill();
                    StartCoroutine(ShowDeviceEnd(1.0f));
                }

                break;
            case SystemType.CLOAK:
                if (!buffer)
                {
                    buffer = true;
                    Vector3 temp = player.transform.position + player.transform.forward * 100;
                    enemy1 = Instantiate(Resources.Load("Tutorial/BasicEnemy"), temp,new Quaternion()) as GameObject;
                    enemy1.transform.parent = GameObject.Find("Enemy").transform;
                    
                    ClearText();
                    s1 = "Cloak can make you invisible to the enemies and slows down time.s";
                    s2 = "It is very useful when you need to avoid battle.";
                    s3 = "Try to use it!";
                    StartCoroutine(Delay(0.0f, s1, s2, s3));
                    player.StopMovement();
                }
                //if (Vector3.Distance(enemy1.transform.position, player.transform.position) <= 40)
                //{
                //    enemy1.transform.position = player.transform.position + player.transform.forward * 100;
                //}
                if (systemManager.GetActive(SystemType.CLOAK))
                {
                    StartCoroutine(ShowDeviceEnd(2.0f));
                    Invoke("DestroyEnemy", 2f);
                }
                break;
            case SystemType.RADAR:
                if (!buffer)
                {
                    buffer = true;
                    Vector3 temp = player.transform.position + player.transform.forward * 500;
                    enemy1 = Instantiate(Resources.Load("Tutorial/BasicEnemy"), temp, Quaternion.identity) as GameObject;
                    enemy1.transform.parent = GameObject.Find("Enemy").transform;

                    ClearText();
                    s1 = "Nearby enemies and loot will appear on the radar.";
                    s2 = "You can pick up the radar by scooping it up with your right hand";
                    s3 = "Relocate radar to where you prefer by closing your hand";
                    StartCoroutine(Delay(0.0f, s1, s2, s3));
                    player.StopMovement();
                    Invoke("DestroyEnemy", 15.0f);
                    StartCoroutine(ShowDeviceEnd(15.0f));
                }
                
                break;
            case SystemType.DECOY:
                if (!buffer)
                {
                    ClearText();
                    s1 = "You can lure the enemies away by spawning a decoy.";
                    s2 = "Using decoy with the cloak can ease up your journey.";
                    s3 = "Try to use it!";
                    buffer = true;
                    StartCoroutine(Delay(0.0f, s1, s2, s3));
                    player.StopMovement();
                }
                else if (systemManager.GetActive(SystemType.DECOY))
                    StartCoroutine(ShowDeviceEnd(1.0f));

                break;
            case SystemType.LASERS:
                if (!buffer)
                {
                    Vector3 temp = player.transform.position + player.transform.forward * 150;
                    enemy1 = Instantiate(Resources.Load("Tutorial/BasicEnemy"), temp, Quaternion.identity) as GameObject;
                    enemy1.transform.parent = GameObject.Find("Enemy").transform;
                    
                    ClearText();
                    s1 = "Laser is a fast pace weapon.";
                    s2 = "Try to use the laser to eliminate the enemy.";
                    s3 = "Press right trigger to shoot laser";
                    buffer = true;
                    StartCoroutine(Delay(0.0f, s1, s2, s3));
                    player.StopMovement();
                }
                if(enemy1)
                    Arrow.transform.LookAt(enemy1.transform.position);
                else
                {
                    StartCoroutine(ShowDeviceEnd(1.0f));
                    //Invoke("DestroyEnemy", 2f);
                }
                break;

            case SystemType.MISSILES:
                if (!buffer)
                {
                    Vector3 temp = player.transform.position + player.transform.forward * 150;
                    enemy1 = Instantiate(Resources.Load("Tutorial/BasicEnemy"), temp, Quaternion.identity) as GameObject;
                    enemy1.transform.parent = GameObject.Find("Enemy").transform;

                    ClearText();
                    s1 = "Missile is a powerful weapon with long range.";
                    s2 = "Try to fire a missile to eliminate the enemy.";
                    s3 = "Press Left Bumper to fire a missile";
                    buffer = true;
                    StartCoroutine(Delay(0.0f, s1, s2, s3));
                    player.StopMovement();
                }
                if (enemy1)
                    Arrow.transform.LookAt(enemy1.transform.position);
                else
                { 
                    StartCoroutine(ShowDeviceEnd(1.0f));
                    //Invoke("DestroyEnemy", 1.5f);
                }
                break;

            default:
                break;
        }
    }

    private IEnumerator ShowDeviceEnd(float length)
    {
        if (type != SystemType.RADAR)
            type = SystemType.NONE;
        
        yield return new WaitForSeconds(length);
        type = SystemType.NONE;
        playerMovement.enabled = true;
        //ClearText();
        buffer = false;
    }

    private Vector3 GetClosestShipPart()
    {
        Vector3 target = Vector3.zero;
        float distance = float.MaxValue;
        for (int i = 0; i < shipParts.Length; i++)
        {
            if (!shipParts[i])
            {
                continue;
            }
            else if (Vector3.Distance(shipParts[i].transform.position, player.transform.position) < distance)
            {
                target = shipParts[i].transform.position;
            }
        }
        return target;
    }

    public void IncreamentDevice(SystemType _type)
    {
        deviceCollected++;
        type = _type;
    }

    public void MissionCompleted()
    {
        missionCompleted = true;
    }
    public void MissionAccepted()
    {
        missionAccepted = true;
    }

    public void MissionTurnedIn()
    {
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

    void DestroyEnemy()
    {
        if (enemy1)
        {
            enemy1.GetComponent<TutorialEnemy>().Kill();
            enemy1 = null;
        }
    }
}
