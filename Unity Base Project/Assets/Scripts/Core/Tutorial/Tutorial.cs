using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using GD.Core.Enums;

public class Tutorial : MonoBehaviour
{

    private int phase;
    private HeadMovement headMovement;
    private Thrusters thruster;
    private PlayerMovement playerMovement;
    private MissionSystem mission;
    private Text line1, line2, line3;
    private int deviceCollected, enemyKilled;
    private SystemType type;
    private PlayerStats player;
    public GameObject Arrow;
    bool buffer;
    private GameObject[] shipParts;
    public GameObject enemyShip;
    private GameObject enemy1 = null, enemy2 = null;


    // Use this for initialization
    void Start()
    {
        headMovement = GameObject.Find("CenterEyeAnchor").GetComponent<HeadMovement>();
        mission = GameObject.Find("PersistentGameObject").GetComponent<MissionSystem>();
        player = GameObject.Find("Player").GetComponent<PlayerStats>();
        thruster = GameObject.Find("Thruster").GetComponent<Thrusters>();
        shipParts = GameObject.FindGameObjectsWithTag("Loot");
        //enemyShip = GameObject.FindGameObjectWithTag("Enemy");

        line1 = GameObject.Find("Line1").GetComponent<Text>();
        line2 = GameObject.Find("Line2").GetComponent<Text>();
        line3 = GameObject.Find("Line3").GetComponent<Text>();
        line1.text = "Welcome, Captain!";
        line2.text = "";
        line3.text = "";
        phase = 0;
        deviceCollected = 0;
        enemyKilled = 0;
        buffer = false;
        Arrow.SetActive(false);
        type = SystemType.NONE;
    }

    // Update is called once per frame
    void Update()
    {
        //Progress();
        ProgressTest();
    }

    //void Progress()
    //{
    //    string string1, string2, string3;
    //    switch (phase)
    //    {
    //        case 0:
    //            //Arrow.SetActive(true);
    //            //Arrow.transform.LookAt(GetClosestShipPart());
    //            if (!buffer)
    //            {
    //                headMovement.enabled = false;
    //                thruster.enabled = false;
    //                string1 = "Please Open the mission log to accept \"Scarvenger\"";
    //                string2 = "Touch your left forearm with your right Index to open control board";
    //                string3 = "Use your right index to select/use items";
    //                StartCoroutine(Delay(5.0f, string1, string2, string3));
    //                buffer = true;
    //            }

    //            if (mission.m_LevelMissions[0].isActive)
    //            {
    //                ClearText();
    //                line1.text = "Good Job!";
    //                headMovement.enabled = true;
    //                thruster.enabled = true;
    //                phase++;
    //                buffer = false;
    //            }
    //            break;

    //        case 1:
    //            if (!buffer)
    //            {
    //                string1 = "Push the handle on your left to start the ship";
    //                string2 = "You may rotate your head to change direction";
    //                string3 = "";
    //                StartCoroutine(Delay(2.0f, string1, string2, string3));
    //                buffer = true;
    //            }

    //            if (player.GetMoveData().Speed > 0)
    //            {
    //                ClearText();
    //                phase++;
    //                buffer = false;
    //                Arrow.SetActive(true);
    //            }

    //            break;

    //        case 2:
    //            line1.text = "Collect 6 ship parts to form a complete ship";
    //            line2.text = "Completion (" + mission.m_LevelMissions[0].objectives + " / 6)";
    //            line3.text = "";
    //            Arrow.transform.LookAt(GetClosestShipPart());
    //            if (mission.m_LevelMissions[0].objectives == 6)
    //            {
    //                Arrow.SetActive(false);
    //                phase++;
    //            }
    //            break;
    //        case 3:
    //            if (!buffer)
    //            {
    //                string1 = "Congradulations!";
    //                string2 = "You may return to the Station and turn in the mission!";
    //                string3 = "";
    //                StartCoroutine(Delay(3.0f, string1, string2, string3));
    //                buffer = true;
    //            }

    //            if (mission.m_LevelMissions[0].completed)
    //            {
    //                buffer = false;
    //                ClearText();
    //                phase++;
    //            }
    //            break;

    //        case 4:
    //            if (!buffer)
    //            {
    //                string1 = "Please Accept \"Kill....\" from the mission log";
    //                string2 = "Touch your left forearm with your right Index to open control board";
    //                string3 = "Use your right index to select items";
    //                StartCoroutine(Delay(0.0f, string1, string2, string3));
    //                buffer = true;
    //            }

    //            if (mission.m_LevelMissions[1].isActive)
    //            {
    //                buffer = false;
    //                ClearText();
    //                phase++;
    //                Arrow.SetActive(true);
    //                GameObject.Instantiate(enemyShip, Vector3.zero, Quaternion.identity);
    //            }
    //            break;

    //        case 5:
    //            Arrow.transform.LookAt(enemyShip.transform.position);
    //            line1.text = "Kill the enemy ship";
    //            line2.text = "Completion (" + mission.m_LevelMissions[1].objectives + " / 1)";
    //            line3.text = "You can use either missile or laser to do the job!";
    //            if (mission.m_LevelMissions[1].objectives == 1)
    //            {
    //                phase++;
    //            }
    //            break;

    //        case 6:
    //            if (!buffer)
    //            {
    //                string1 = "Congratulations!";
    //                string2 = "You may return to the Station and turn in the mission!";
    //                string3 = "";
    //                StartCoroutine(Delay(2.0f, string1, string2, string3));

    //                string1 = "";
    //                string2 = "";
    //                string3 = "";
    //                StartCoroutine(Delay(1.0f, string1, string2, string3));

    //                string1 = "You must also watch your surrounding";
    //                string2 = "Some environment can be toxic and deadly, such as Nebula Clouds";
    //                string3 = "However you can also randomly relocate yourself by going through a wormhole";
    //                StartCoroutine(Delay(3.0f, string1, string2, string3));
    //                buffer = true;
    //            }

    //            if (mission.m_LevelMissions[1].completed)
    //            {
    //                phase++;
    //                ClearText();
    //            }
    //            break;

    //        case 7:
    //            StartCoroutine(Transition());
    //            break;

    //        default:
    //            break;
    //    }
    //}

    void ProgressTest()
    {
        string string1, string2, string3;
        switch (phase)
        {
            case 0:
                if (!buffer)
                {
                    string1 = "Push the handle on your left to start the ship";
                    string2 = "You may rotate your head to change direction";
                    string3 = "";
                    StartCoroutine(Delay(2.0f, string1, string2, string3));
                    buffer = true;
                }

                if (player.GetMoveData().Speed > 0)
                {
                    ClearText();
                    phase++;
                    buffer = false;
                    Arrow.SetActive(true);
                }

                break;

            case 1:
                if (type != SystemType.NONE)
                {
                    playerMovement.enabled = false;
                    //playerMovement.SendMessage();
                    ShowDevice();
                }
                else
                {
                    line1.text = "Collect 6 ship parts to form a complete ship";
                    line2.text = "Completion (" + deviceCollected + " / 6)";
                    line3.text = "";
                    Arrow.transform.LookAt(GetClosestShipPart());

                    if (deviceCollected == 6)
                    {
                        Arrow.SetActive(false);
                        ClearText();
                        phase++;
                    }
                }
                break;

            case 3:
                if (!buffer)
                {
                    string1 = "You must also watch out your surrounding";
                    string2 = "Some environment can be toxic and deadly, such as Nebulas Clous";
                    string3 = "However you can also randomly relocate youself by going through a wormhole";
                    StartCoroutine(Delay(0.0f, string1, string2, string3));
                    buffer = true;
                }
                break;

            case 7:
                StartCoroutine(Transition());
                break;

            default:
                break;
        }
    }

    IEnumerator Delay(float length, string s1, string s2, string s3)
    {
        yield return new WaitForSeconds(length);
        line1.text = s1;
        yield return new WaitForSeconds(0.5f);
        line2.text = s2;
        yield return new WaitForSeconds(0.5f);
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
        switch (type)
        {
            case SystemType.EMP:
                if (!buffer)
                {
                    ClearText();
                    s1 = "You can stun the surrounding enemies with EMP";
                    s2 = "EMP is also extremely effective against the droid.";
                    s3 = "It will make them self explode";
                    buffer = true;
                    StartCoroutine(Delay(0.0f, s1, s2, s3));
                }
                else
                    StartCoroutine(ShowDeviceEnd(5.0f));

                break;
            case SystemType.CLOAK:
                if (!buffer)
                {
                    ClearText();
                    s1 = "Cloak can make you invisible to the enemies.";
                    s2 = "It is very useful when you need to avoid battle.";
                    s3 = "It only last 20 seconds, use it wisely";
                    buffer = true;
                    StartCoroutine(Delay(0.0f, s1, s2, s3));
                }
                else
                    StartCoroutine(ShowDeviceEnd(5.0f));

                break;
            case SystemType.RADAR:
                if (!buffer)
                {
                    ClearText();
                    s1 = "Nearby enemies and loot will appear on the radar.";
                    s2 = "You can pick up the radar with your hand open.";
                    s3 = "Relocate radar to where you prefer by closing your hand";
                    buffer = true;
                    StartCoroutine(Delay(0.0f, s1, s2, s3));
                }
                else
                    StartCoroutine(ShowDeviceEnd(5.0f));

                break;
            case SystemType.DECOY:
                if (!buffer)
                {
                    ClearText();
                    s1 = "You can lure the enemies away by spawning a decoy.";
                    s2 = "Using decoy with the cloak can ease up your journey.";
                    s3 = "Be smart, be strategic.";
                    buffer = true;
                    StartCoroutine(Delay(0.0f, s1, s2, s3));
                }
                else
                    StartCoroutine(ShowDeviceEnd(5.0f));

                break;
            case SystemType.LASERS:
                if (!buffer)
                {
                    enemy1 = GameObject.Instantiate(enemyShip, Vector3.zero, Quaternion.identity) as GameObject;
                    enemy1.transform.parent = GameObject.Find("Enemy").transform;
                    enemy1.GetComponent<EnemyBehavior>().enabled = false;
                    enemy1.GetComponent<PatrolAi>().enabled = false;
                    ClearText();
                    s1 = "Laser is a fast pase weapon.";
                    s2 = "Try to use the laser to eliminate the enemy.";
                    s3 = "";
                    buffer = true;
                    StartCoroutine(Delay(0.0f, s1, s2, s3));
                }
                else
                    Arrow.transform.LookAt(enemy1.transform.position);

                if (!enemy1 && buffer)
                {
                    StartCoroutine(ShowDeviceEnd(5.0f));
                }
                break;

            case SystemType.MISSILES:
                if (!buffer)
                {
                    enemy2 = GameObject.Instantiate(enemyShip, Vector3.zero, Quaternion.identity) as GameObject;
                    enemy2.transform.parent = GameObject.Find("Enemy").transform;
                    enemy2.GetComponent<EnemyBehavior>().enabled = false;
                    enemy2.GetComponent<PatrolAi>().enabled = false;
                    ClearText();
                    s1 = "Missile is a powerful weapon with long range.";
                    s2 = "Try to fire a missile to eliminate the enemy.";
                    s3 = "";
                    buffer = true;
                    StartCoroutine(Delay(0.0f, s1, s2, s3));
                }
                else
                    Arrow.transform.LookAt(enemy2.transform.position);

                if (!enemy2 && buffer)
                {
                    StartCoroutine(ShowDeviceEnd(5.0f));
                }
                break;

            default:
                break;
        }
    }

    private IEnumerator ShowDeviceEnd(float length)
    {
        yield return new WaitForSeconds(length);
        playerMovement.enabled = true;
        ClearText();
        buffer = false;
        type = SystemType.NONE;
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
}
