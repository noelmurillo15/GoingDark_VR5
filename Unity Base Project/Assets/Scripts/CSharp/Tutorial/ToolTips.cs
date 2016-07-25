using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ToolTips : MonoBehaviour
{
    public struct Tip
    {
        public string name;
        public string text;
        public Tip(string s, string t) { name = s; text = t; }
    }
    List<Tip> list = new List<Tip>();
    Tip tip;
    string text;
    private Text line1, line2;
    private bool buffer;
    // Use this for initialization
    void Start()
    {
        line1 = GameObject.Find("Line1").GetComponent<Text>();
        line2 = GameObject.Find("Line2").GetComponent<Text>();

        buffer = true;
        Initialize();
    }

    void Initialize()
    {
        text = "Hold the LEFT TRIGGER to accelerate the ship. Use the LEFT STICK to rotate the ship.";
        tip = new Tip("Movement", text);
        list.Add(tip);

        text = "Your Ship is equipped with Lasers, hold the RIGHT TRIGGER to use them. Change the laser type by clicking the RIGHT JOYSTICK.";
        tip = new Tip("Laser", text);
        list.Add(tip);

        text = "You also have Missiles which can be launched by pressing the RIGHT BUMPER";
        tip = new Tip("Missile", text);
        list.Add(tip);

        text = "You have access to four types of Missiles. Press Y to swap between them";
        tip = new Tip("Missile", text);
        list.Add(tip);

        text = "Cloak can make you invisible to enemies for an indefinite amount of time. Press X to activate cloak.";
        tip = new Tip("Cloak", text);
        list.Add(tip);

        text = "Sending Out a decoy can distract most enemies away from you. Press B to deploy one";
        tip = new Tip("Decoy", text);
        list.Add(tip);

        text = "Sending Out a decoy can distract most enemies away from you. Press B to deploy one";
        tip = new Tip("Decoy", text);
        list.Add(tip);

        text = "Pressing the LEFT BUMPER will initialize the Hyper drive sequence giving you a increased speed for a limited time";
        tip = new Tip("Hyperdrive", text);
        list.Add(tip);

        text = "Pressing A will send out an EMP shock wave stunning all nearby enemies for a short time. The EMP will also kill any Droids";
        tip = new Tip("Electro-Magnetic Pulse", text);
        list.Add(tip);

        text = "You must complete the main missions in order to move on to the next level.";
        tip = new Tip("Main Mission", text);
        list.Add(tip);

        text = "Completing Optional Missions is a good way to gain resource and enhance your skills!";
        tip = new Tip("Optional Mission", text);
        list.Add(tip);
    }

    // Update is called once per frame
    void Update()
    {
        if (buffer)
          StartCoroutine(Show());
    }

    IEnumerator Show()
    {
        buffer = false;
        for (int i = 0; i < list.Count-1; i++)
        {
            line1.text = list[i].name;
            line2.text = list[i].text;
            AudioManager.instance.PlayMessagePop();
            yield return new WaitForSeconds(12f);
        }
        buffer = true;
    }

    public void InsertTip(Tip t,int i)
    {
        list.Insert(i, t);
    }

    public void AddTip(Tip t)
    {
        list.Add(t);
    }
}
