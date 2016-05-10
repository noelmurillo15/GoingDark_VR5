using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissileDisplay : MonoBehaviour
{

    public int missCounter;
    private GameObject messages;
    private float timer;
    [SerializeField]
    private Text textCount;

    // Use this for initialization
    void Start()
    {
        messages = GameObject.Find("Player");
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //timedMessage();

        SendMessage("GetMissileCount");

        if (timer <= 0f)
            SendMessage("GetMissileCount");
        else
            timer = 5.0f;

        if (timer >= 5.0f)
            timer -= Time.deltaTime;

        textCount.text = missCounter.ToString();

    }
    public void GetMissileCount()
    {
        missCounter = messages.GetComponent<PlayerStats>().GetNumMissiles();
    }

    IEnumerator timedMessage()
    {
        SendMessage("GetMissileCount");
        yield return new WaitForSeconds(2.0f);
    }
}
