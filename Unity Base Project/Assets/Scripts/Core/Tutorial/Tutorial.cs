using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tutorial : MonoBehaviour {

    private int phase;
    private HeadMovement headMovement;
    private MissionSystem mission;
    private Text line1, line2, line3;
    private int deviceCollected;
    private PlayerStats player;
    bool buffer;
    //private HyperdriveSystem hyperDrvie;

	// Use this for initialization
	void Start () {
        headMovement = GameObject.Find("CenterEyeAnchor").GetComponent<HeadMovement>();
        mission = GameObject.Find("PersistentGameObject").GetComponent<MissionSystem>();
        player = GameObject.Find("Player").GetComponent<PlayerStats>();
        
        //hyperDrvie = GameObject.Find("Devices").GetComponentInChildren<HyperdriveSystem>();
        line1 = GameObject.Find("Line1").GetComponent<Text>();
        line2 = GameObject.Find("Line2").GetComponent<Text>();
        line3 = GameObject.Find("Line3").GetComponent<Text>();
        line1.text = "Welcome, Captain!";
        line2.text = "";
        line3.text = "";
        phase = 0;
        deviceCollected = 0;
        buffer = false;
    }
	
	// Update is called once per frame
	void Update () {

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
                    headMovement.enabled = false;
                    string1 = "Please Open the mission log to accept \"Scarvenger\"";
                    string2 = "Touch your left forearm with your right Index to open control board";
                    string3 = "Use your right index to select items";
                    StartCoroutine(Delay(5.0f, string1, string2, string3));
                    //hyperDrvie.enabled = false;
                    buffer = true;
                }

                if (mission.m_LevelMissions[0].isActive)
                {
                    ClearText();
                    line1.text = "Good Job!";
                    headMovement.enabled = true;
                    phase++;
                    buffer = false;
                }
                break;

            case 1:
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
                    //hyperDrvie.enabled = true;
                }

                break;

            case 2:

                line1.text = "Collect 6 ship parts to form a complete ship";
                line2.text = "Completion (" + deviceCollected + " / 6)";
                line3.text = "";
                //StartCoroutine(Delay(1.0f, string1, string2, string3));

                if (deviceCollected == 6)
                {
                    phase++;
                }
                break;
            case 3:
                if (!buffer)
                {
                    string1 = "Congradulations!";
                    string2 = "You may return to the Station and turn in the mission!";
                    string3 = "";
                    StartCoroutine(Delay(3.0f, string1, string2, string3));
                    buffer = true;
                }
                
                if (mission.m_LevelMissions[0].completed)
                {
                    Destroy(this);
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
        yield return new WaitForSeconds(0.5f);
        line2.text = s2;
        yield return new WaitForSeconds(0.5f);
        line3.text = s3;
    }

    private void ClearText()
    {
        line1.text = "";
        line2.text = "";
        line3.text = "";
    }

    public void IncreamentDevice()
    {
        deviceCollected++;
    }
}
