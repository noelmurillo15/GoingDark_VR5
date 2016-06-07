using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthProperties : MonoBehaviour
{

    [SerializeField]
    private GameObject[] health;
    [SerializeField]
    public int hitCount { get; set; }
    [SerializeField]
    private PlayerStats stats;
    [SerializeField]
    private int ArmorStage;
    GameObject PlayerShip;
    public CameraShake camShake;


    // Use this for initialization
    void Start()
    {
        hitCount = 0;
        camShake = GameObject.FindGameObjectWithTag("LeapMount").GetComponent<CameraShake>();
        stats = GameObject.Find("Player").GetComponent<PlayerStats>();
        PlayerShip = GameObject.FindGameObjectWithTag("PlayerShip");
        health = new GameObject[6];
        ArmorStage = 5;
        //setting active or inactive
        //grabs all the health bars
        for (int x = 0; x < transform.childCount; x++)
        {
            health[x] = transform.GetChild(x).gameObject;
        }
        UpdateHealthBars();
        //update
        UpdatePlayerHealth();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdatePlayerHealth()
    {
        //loop through all the the health bars
        for (int i = 0; i < ArmorStage; i++)
        {
            if (i < hitCount)//if you are hit start turning them red.
                health[i].gameObject.GetComponent<Renderer>().material.color = Color.red;
            else // keep them green
                health[i].gameObject.GetComponent<Renderer>().material.color = Color.green;
        }

    }

    public void SetArmorStage(int NewArmorStage)
    {
        if (NewArmorStage >= 2 && NewArmorStage <= 5)
        {
            ArmorStage = NewArmorStage;
            UpdateHealthBars();
            UpdatePlayerHealth();
        }
    }

    public int GetArmorStage() { return ArmorStage; }

    public void UpdateHealthBars()
    {


        switch (ArmorStage)
        {
            case 2:
                for (int i = 2; i < 5; i++)
                {
                    health[i].SetActive(false);
                }
                break;
            case 3:
                health[2].SetActive(true);
                for (int i = 3; i < 5; i++)
                    health[i].SetActive(false);
                break;
            case 4:
                for (int i = 0; i < 5; i++)
                    health[i].SetActive(true);
                health[4].SetActive(false);
                break;
            case 5:
                for (int i = 0; i < 5; i++)
                    health[i].SetActive(true);
                break;
            default:
                for (int i = 2; i < 5; i++)
                    health[i].SetActive(false);
                break;
        }
       // PlayerShip.SendMessage("ChangeArmor", ArmorStage);
    }

    public void Kill()
    {
        Debug.Log("Destroyed Player Ship");
        stats.SendMessage("Kill");
       // SceneManager.LoadScene("GameOver");
    }

    public void Hit()
    {
        AudioManager.instance.PlayHit();
        hitCount++;
        camShake.PlayShake();
        UpdatePlayerHealth();
        if (hitCount >= ArmorStage)
            Kill();
    }

    public void EnvironmentalDMG()
    {
        hitCount++;
        UpdatePlayerHealth();
        if (hitCount >= ArmorStage)
            Kill();
    }
}
