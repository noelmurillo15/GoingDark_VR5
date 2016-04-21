using UnityEngine;
using System.Collections;

public class MessagingScript : MonoBehaviour
{
    public int msgType;
    public float timer;
    public Transform Navmessenger;
    public Transform Mario;
    public Transform NoEnemyDetect;
    public Transform AsteroidWarning;

    enum Messages { NAV = 0, EN_FOUND, NO_EN_FOUND, ASTEROID };

    // Use this for initialization
    void Start()
    {
        

        if (Navmessenger == null)
            Navmessenger = GameObject.Find("Msg").transform;

        if (Mario == null)
            Mario = GameObject.Find("Mario").transform;

        if (NoEnemyDetect == null)
            NoEnemyDetect = GameObject.Find("NoEnemyFound").transform;

        if (AsteroidWarning == null)
            AsteroidWarning = GameObject.Find("AsteroidWarning").transform;

        //init  msg scale 
        Navmessenger.transform.localScale.Set(0.1f, 0.1f, 0.1f);
        Mario.transform.localScale.Set(0.1f, 0.1f, 0.1f);
        NoEnemyDetect.transform.localScale.Set(0.1f, 0.1f, 0.1f);
        AsteroidWarning.transform.localScale.Set(0.1f, 0.1f, 0.1f);
        msgType = 0;
        UpdateMessage();
        timer = 5.0f;

    }


    // Update is called once per frame
    void Update()
    {
        //Navmessenger.gameObject.SetActive(true);
        //Test Timer
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = 10.0f;
           // msgType++;
            UpdateMessage();

        }

    }

    int GetMsgType()
    {
        return msgType;
    }

    int SetMsgType(int msg)
    {
        msgType = msg;
        return msgType;
    }

    void UpdateMessage()
    {

        switch (msgType)
        {
            case 0:
                timer = 5;
                Navmessenger.gameObject.SetActive(true);
                Mario.gameObject.SetActive(false);
                NoEnemyDetect.gameObject.SetActive(false);
                AsteroidWarning.gameObject.SetActive(false);
                break;

            case 1:
                Navmessenger.gameObject.SetActive(true);
                //timer = 5;

                // Navmessenger.gameObject.SetActive(false);
                Mario.gameObject.SetActive(false);
                NoEnemyDetect.gameObject.SetActive(false);
                AsteroidWarning.gameObject.SetActive(false);
                //DestroyObject(Navmessenger);
                if (msgType != 1)
                {
                    Navmessenger.gameObject.SetActive(false);
                }

                break;

            case 2:
                Mario.gameObject.SetActive(true);
                //timer = 5;

                Navmessenger.gameObject.SetActive(false);
                //EnemyFound.gameObject.SetActive(false);
                NoEnemyDetect.gameObject.SetActive(false);
                AsteroidWarning.gameObject.SetActive(false);
                if (msgType != 2)
                {
                    Mario.gameObject.SetActive(false);
                }

                break;

            case 3:
                NoEnemyDetect.gameObject.SetActive(true);
                //timer = 5;
                Navmessenger.gameObject.SetActive(false);
                Mario.gameObject.SetActive(false);
                //NoEnemyDetect.gameObject.SetActive(false);
                AsteroidWarning.gameObject.SetActive(false);
                if (msgType != 3)
                {
                    NoEnemyDetect.gameObject.SetActive(false);
                }
                break;

            case 4:
                //AsteroidWarning.gameObject.SetActive(true);
                //timer = 5;
                Navmessenger.gameObject.SetActive(false);
                Mario.gameObject.SetActive(true);
                NoEnemyDetect.gameObject.SetActive(false);
                // AsteroidWarning.gameObject.SetActive(false);
                if (msgType != 4)
                {
                    Mario.gameObject.SetActive(false);
                }
                break;
            case 5:

               // msgType = 0;
                break;
        }
    }
}
