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
        text = "Push the LEFT TRIGGER to accelerate the ship. Use the LEFT STICK to rotate the ship.";
        tip = new Tip("Movement", text);
        list.Add(tip);

        text = "Touch the item you want to select with your hand.";
        tip = new Tip("Select", text);
        list.Add(tip);

        text = "Clap your hands to open the Arm Menu.";
        tip = new Tip("Arm Menu", text);
        list.Add(tip);

        text = "You can check the current actived missions in the mission log.";
        tip = new Tip("Mission Log", text);
        list.Add(tip);

        text = "You may repair your ship, as well as accept/turn in the main mission in the station.";
        tip = new Tip("Station", text);
        list.Add(tip);

        text = "You must complete the main missions in order to move on to the next level.";
        tip = new Tip("Main Mission", text);
        list.Add(tip);

        text = "Complete Optional Missions is a good way to gain resource and sharp your skills!";
        tip = new Tip("Optional Mission", text);
        list.Add(tip);

        text = "Laser is a fast-firing weapon, but it will overheat if used too repetitively. You can hold RIGHT TRIGGER to fire.";
        tip = new Tip("Laser", text);
        list.Add(tip);

        text = "Missile is a strong consumable weapon, you can recharge it in many ways however. Press RIGHT BUMPER to fire a missile";
        tip = new Tip("Missile1", text);
        list.Add(tip);

        text = "Missile also comes with different forms that you can purchase in the shop, each of them have different powers. You can swap the form with Y BUTTON.";
        tip = new Tip("Missile2", text);
        list.Add(tip);

        text = "Cloak can make you invisible to enemies, but only for a short amount of time. Press X BUTTON to activate cloak.";
        tip = new Tip("Cloak", text);
        list.Add(tip);

        text = "Decoy can lure the enemies away from you, Press B BUTTON to send out a decoy.";
        tip = new Tip("Decoy", text);
        list.Add(tip);

        text = "Your lasers and missiles can break asteroids into smaller pieces. Your ship will be damaged upon fast collision with asteroids.";
        tip = new Tip("Asteroids", text);
        list.Add(tip);

        text = "Enemies are the major threat to you, but they can also drop useful resource.";
        tip = new Tip("Enemies", text);
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
            yield return new WaitForSeconds(10f);
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
