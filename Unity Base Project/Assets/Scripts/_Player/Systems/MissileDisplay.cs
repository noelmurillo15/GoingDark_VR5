using UnityEngine;
using UnityEngine.UI;

public class MissileDisplay : MonoBehaviour
{

    public int missCounter;
    //player stats
    private GameObject pStat;
    private float timer;
    [SerializeField]
    private Text textCount;

    // Use this for initialization
    void Start()
    {
        pStat = GameObject.Find("Player");

        timer = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0f)
            GetMissileCount();
        else
            timer = 5.0f;

        if (timer >= 5.0f)
            timer -= Time.deltaTime;

        textCount.text = missCounter.ToString();
    }

    public void GetMissileCount()
    {
        missCounter = GameObject.FindGameObjectWithTag("Systems").GetComponent<MissileSystem>().Count;
    }    
}
