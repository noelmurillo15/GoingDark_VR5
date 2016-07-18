using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using GoingDark.Core.Enums;

public class Tutorial : MonoBehaviour
{

    //private int phase;
    //private PlayerMovement playerMovement;
    //private MissionSystem mission;
    //private Text line1, line2, line3;
    //private int deviceCollected;
    //private SystemType type;
    //private PlayerMovement player;
    //public GameObject Arrow;
    //private bool buffer, missionCompleted, missionAccepted, missionTurnedIn, isNearStation;
    //private GameObject[] shipParts;
    //private GameObject enemy1 = null, droidBot = null;
    //private SystemManager systemManager;
    //private GameObject station;
    //private HyperdriveSystem hyperDrive;
    //private GameObject hyperDriveParticle;
    //private BoxCollider hyperDriveButton;
    //private GameObject decoyButtonParticle, hyperDriveButtonParticle, cloakButtonParticle, empButtonParticle;
    //private GameObject[] loots;
    //private AudioSource tutorialRemind;
    //float remindTimer;

    //// Use this for initialization
    //void Start()
    //{
    //    mission = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MissionSystem>();
    //    player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    //    shipParts = GameObject.FindGameObjectsWithTag("Loot");
    //    playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    //    systemManager = GameObject.Find("Devices").GetComponent<SystemManager>();
    //    station = GameObject.Find("Station");
    //    hyperDrive = GameObject.Find("HyperDrive(Clone)").GetComponent<HyperdriveSystem>();
    //    hyperDriveParticle = GameObject.Find("WarpDriveParticles");
    //    hyperDriveButton = GameObject.Find("HyperdriveButton").GetComponent<BoxCollider>();
    //    decoyButtonParticle = GameObject.Find("DecoyButtonParticle");
    //    empButtonParticle = GameObject.Find("EmpButtonParticle");
    //    cloakButtonParticle = GameObject.Find("CloakButtonParticle");
    //    hyperDriveButtonParticle = GameObject.Find("HyperButtonParticle");
    //    loots = GameObject.FindGameObjectsWithTag("Loot");
    //    tutorialRemind = GetComponent<AudioSource>();

    //    line1 = GameObject.Find("Line1").GetComponent<Text>();
    //    line2 = GameObject.Find("Line2").GetComponent<Text>();
    //    line3 = GameObject.Find("Line3").GetComponent<Text>();
    //    line1.text = "Welcome, Captain!";
    //    line2.text = "";
    //    line3.text = "";
    //    phase = 0;
    //    deviceCollected = 0;
    //    remindTimer = 6.0f;
    //    buffer = false;
    //    missionAccepted = false;
    //    missionCompleted = false;
    //    missionTurnedIn = false;
    //    Arrow.SetActive(false);
    //    type = SystemType.Hyperdrive;
    //    isNearStation = false;
    //    decoyButtonParticle.SetActive(false);
    //    empButtonParticle.SetActive(false);
    //    cloakButtonParticle.SetActive(false);
    //    hyperDriveButtonParticle.SetActive(false);
    //    for (int i = 0; i < loots.Length; i++)
    //    {
    //        loots[i].SetActive(false);
    //    }
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    Progress();
    //}

    //void Progress()
    //{
    //    string string1, string2, string3;
    //    switch (phase)
    //    {
    //        case 0:
    //            if (!buffer)
    //            {
    //                string1 = "Push the left trigger to accelerate the ship";
    //                string2 = "Use the left stick to rotate the ship";
    //                string3 = "Drive to the station (Hint: Follow the arrow)";
    //                StartCoroutine(Delay(2.0f, string1, string2, string3));
    //                buffer = true;
    //                Arrow.SetActive(true);
    //            }
    //            TutorialRemind();
    //            Arrow.transform.LookAt(station.transform);
    //            if (!hyperDriveParticle.activeSelf)
    //            {
    //                hyperDrive.enabled = false;
    //                hyperDriveButton.enabled = false;
    //            }

    //            if (isNearStation)
    //            {
    //                ClearText();
    //                playerMovement.enabled = false;
    //                player.StopMovement();
    //                buffer = false;
    //                phase++;
    //                StopTutorialRemind();
    //            }
    //            break;
    //        case 1:
    //            if (!buffer)
    //            {
    //                string1 = "Touch your left forearm with your right palm to open the Arm Menu";
    //                string2 = "Click the mission log button";
    //                string3 = "Accept the first mission : " + mission.m_stationMissions[0][0].missionName;
    //                StartCoroutine(Delay(0.0f, string1, string2, string3));
    //                buffer = true;
    //            }
    //            TutorialRemind();

    //            if (missionAccepted)
    //            {
    //                playerMovement.enabled = true;
    //                ClearText();
    //                phase++;
    //                buffer = false;
    //                for (int i = 0; i < loots.Length; i++)
    //                {
    //                    //loots[i].GetComponent<BoxCollider>().enabled = true;
    //                    loots[i].SetActive(true);

    //                }
    //                StopTutorialRemind();

    //            }
    //            break;

    //        case 2:
    //            if (type != SystemType.None)
    //            {
    //                playerMovement.enabled = false;
    //                ShowDevice();
    //            }
    //            else
    //            {
    //                line1.text = "Collect 6 ship parts to form a complete ship";
    //                line2.text = "Completion (" + deviceCollected + " / 6)";
    //                line3.text = "Hint: Follow the arrow";
    //                Arrow.transform.LookAt(GetClosestShipPart());

    //                if (missionCompleted)
    //                {
    //                    ClearText();
    //                    phase++;
    //                }
    //            }
    //            break;
    //        case 3:
    //            if (!buffer)
    //            {
    //                string1 = "Congratulations!";
    //                string2 = "You may return to the Station and turn in the mission!";
    //                string3 = "Hint: Follow the arrow";
    //                StartCoroutine(Delay(3.0f, string1, string2, string3));
    //                buffer = true;
    //            }
    //            TutorialRemind();

    //            if (isNearStation)
    //            {
    //                player.StopMovement();
    //            }
    //                Arrow.transform.LookAt(station.transform);
    //            if (missionTurnedIn)
    //            {
    //                hyperDrive.enabled = true;
    //                hyperDriveButton.enabled = true;
    //                buffer = false;
    //                ClearText();
    //                phase++;
    //                hyperDriveButtonParticle.SetActive(true);
    //                StopTutorialRemind();
    //            }
    //            break;
    //        case 4:
    //            if (!buffer)
    //            {
    //                string1 = "You have completed your training!";
    //                string2 = "Use the hyper drive to exit the scene!";
    //                string3 = "Hyper drive allows you to travel with speed of light";
    //                StartCoroutine(Delay(0.0f, string1, string2, string3));
    //                buffer = true;
    //            }
    //            TutorialRemind();

    //            if (systemManager.GetActive(SystemType.Hyperdrive))
    //            {
    //                ClearText();
    //                buffer = false;
    //                phase++;
    //                hyperDriveButtonParticle.SetActive(false);
    //                StopTutorialRemind();

    //            }
    //            break;

    //        case 5:
    //            if (!buffer)
    //            {
    //                StartCoroutine(Transition());
    //                buffer = true;
    //            }
    //            break;

    //        default:
    //            break;
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
    //    line1.text = "You have completed your training!";
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

    //void ShowDevice()
    //{
    //    string s1, s2, s3;
    //    switch (type)
    //    {
    //        case SystemType.Emp:
    //            if (!buffer)
    //            {
    //                Vector3 temp = player.transform.position + player.transform.forward * 50;
    //                droidBot = Instantiate(Resources.Load("Tutorial/Droid"), temp, Quaternion.identity) as GameObject;
    //                droidBot.transform.parent = GameObject.Find("Enemy").transform;

    //                ClearText();
    //                s1 = "You now have an EMP which slows all enemies nearby";
    //                s2 = "EMP is also extremely effective against droids.";
    //                s3 = "Try to use it on the droid!";
    //                buffer = true;
    //                StartCoroutine(Delay(0.0f, s1, s2, s3));
    //                player.StopMovement();
    //                empButtonParticle.SetActive(true);
    //            }
    //            if (!droidBot)
    //            {
    //                Vector3 temp = player.transform.position + player.transform.forward * 50;
    //                droidBot = Instantiate(Resources.Load("Tutorial/Droid"), temp, Quaternion.identity) as GameObject;
    //                droidBot.transform.parent = GameObject.Find("Enemy").transform;
    //            }
    //            if (systemManager.GetActive(SystemType.Emp) && droidBot)
    //            {
    //                //Invoke("DestroyEnemy", 1f);
    //                //Destroy(droidBot);
    //                droidBot.GetComponent<TutorialEnemy>().Hit();
    //                StartCoroutine(ShowDeviceEnd(1.0f));
    //                empButtonParticle.SetActive(false);
    //            }

    //            break;
    //        case SystemType.Cloak:
    //            if (!buffer)
    //            {
    //                buffer = true;
    //                Vector3 temp = player.transform.position + player.transform.forward * 100;
    //                enemy1 = Instantiate(Resources.Load("Tutorial/BasicEnemy"), temp, new Quaternion()) as GameObject;
    //                enemy1.transform.parent = GameObject.Find("Enemy").transform;

    //                ClearText();
    //                s1 = "Cloak can make you invisible to the enemies and slows down time.s";
    //                s2 = "It is very useful when you need to avoid battle.";
    //                s3 = "Try to use it!";
    //                StartCoroutine(Delay(0.0f, s1, s2, s3));
    //                player.StopMovement();
    //                cloakButtonParticle.SetActive(true);
    //            }
    //            //if (Vector3.Distance(enemy1.transform.position, player.transform.position) <= 40)
    //            //{
    //            //    enemy1.transform.position = player.transform.position + player.transform.forward * 100;
    //            //}
    //            if (systemManager.GetActive(SystemType.Cloak))
    //            {
    //                StartCoroutine(ShowDeviceEnd(2.0f));
    //                Invoke("DestroyEnemy", 2f);
    //                cloakButtonParticle.SetActive(false);
    //            }
    //            break;
    //        case SystemType.Decoy:
    //            if (!buffer)
    //            {
    //                ClearText();
    //                s1 = "You can lure the enemies away by spawning a decoy.";
    //                s2 = "Using decoy with the cloak can ease up your journey.";
    //                s3 = "Try to use it!";
    //                buffer = true;
    //                StartCoroutine(Delay(0.0f, s1, s2, s3));
    //                player.StopMovement();
    //                decoyButtonParticle.SetActive(true);
    //            }
    //            if (systemManager.GetActive(SystemType.Decoy))
    //            {
    //                StartCoroutine(ShowDeviceEnd(1.0f));
    //                decoyButtonParticle.SetActive(false);
    //            }

    //            break;
    //        case SystemType.Laser:
    //            if (!buffer)
    //            {
    //                Vector3 temp = player.transform.position + player.transform.forward * 150;
    //                enemy1 = Instantiate(Resources.Load("Tutorial/BasicEnemy"), temp, Quaternion.identity) as GameObject;
    //                enemy1.transform.parent = GameObject.Find("Enemy").transform;

    //                ClearText();
    //                s1 = "Laser is a fast pace weapon.";
    //                s2 = "Try to use the laser to eliminate the enemy.";
    //                s3 = "Press right trigger to shoot laser";
    //                buffer = true;
    //                StartCoroutine(Delay(0.0f, s1, s2, s3));
    //                player.StopMovement();
    //            }
    //            if(enemy1)
    //                Arrow.transform.LookAt(enemy1.transform.position);
    //            else
    //            {
    //                StartCoroutine(ShowDeviceEnd(1.0f));
    //                //Invoke("DestroyEnemy", 2f);
    //            }
    //            break;

    //        case SystemType.Missile:
    //            if (!buffer)
    //            {
    //                Vector3 temp = player.transform.position + player.transform.forward * 150;
    //                enemy1 = Instantiate(Resources.Load("Tutorial/BasicEnemy"), temp, Quaternion.identity) as GameObject;
    //                enemy1.transform.parent = GameObject.Find("Enemy").transform;

    //                ClearText();
    //                s1 = "Missile is a powerful weapon with long range.";
    //                s2 = "Try to fire a missile to eliminate the enemy.";
    //                s3 = "Press Left Bumper to fire a missile";
    //                buffer = true;
    //                StartCoroutine(Delay(0.0f, s1, s2, s3));
    //                player.StopMovement();
    //            }
    //            if (enemy1)
    //                Arrow.transform.LookAt(enemy1.transform.position);
    //            else
    //            { 
    //                StartCoroutine(ShowDeviceEnd(1.0f));
    //                //Invoke("DestroyEnemy", 1.5f);
    //            }
    //            break;

    //        default:
    //            break;
    //    }
    //}

    //private IEnumerator ShowDeviceEnd(float length)
    //{
    //    type = SystemType.None;

    //    yield return new WaitForSeconds(length);
    //    type = SystemType.None;
    //    playerMovement.enabled = true;
    //    //ClearText();
    //    buffer = false;
    //}

    //private Vector3 GetClosestShipPart()
    //{
    //    Vector3 target = Vector3.zero;
    //    float distance = float.MaxValue;
    //    for (int i = 0; i < shipParts.Length; i++)
    //    {
    //        if (!shipParts[i])
    //        {
    //            continue;
    //        }
    //        else if (Vector3.Distance(shipParts[i].transform.position, player.transform.position) < distance)
    //        {
    //            target = shipParts[i].transform.position;
    //        }
    //    }
    //    return target;
    //}

    //private void TutorialRemind()
    //{
    //    remindTimer += Time.deltaTime;
    //    if (remindTimer>=12f)
    //    {
    //        tutorialRemind.Play();
    //        remindTimer = 0.0f;
    //    }
    //}
    //private void StopTutorialRemind()
    //{
    //    tutorialRemind.Stop();
    //    remindTimer = 6f;
    //}

    //public void IncreamentDevice(SystemType _type)
    //{
    //    deviceCollected++;
    //    type = _type;
    //}

    //public void MissionCompleted(string name)
    //{
    //    if (name == "Scavenger")
    //        missionCompleted = true;
    //}
    //public void MissionAccepted(string name)
    //{
    //    if (name == "Scavenger")
    //        missionAccepted = true;
    //}

    //public void MissionTurnedIn(string name)
    //{
    //    if (name == "Scavenger")
    //        missionTurnedIn = true;
    //}

    //public void EnterStation()
    //{
    //    isNearStation = true;
    //}

    //public void ExitStation()
    //{
    //    isNearStation = false;

    //}

    //void DestroyEnemy()
    //{
    //    if (enemy1)
    //    {
    //        enemy1.GetComponent<TutorialEnemy>().Hit();
    //        enemy1 = null;
    //    }
    //}
}
