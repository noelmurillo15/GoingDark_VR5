using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MessageScript : MonoBehaviour
{
    [SerializeField]
    GameObject winMessage;

    [SerializeField]
    Texture missileImage;
    [SerializeField]
    Texture enemyCloseImage;
    [SerializeField]
    Texture lootPickUpImage;
    [SerializeField]
    Texture enemyMissileComb;
    [SerializeField]
    Texture HUD;

    private Text[] winTexts;

    private float enemyMsgTimer;
    private float missileTimder;
    private GameObject m_missionSystem;

    // Use this for initialization
    void Start()
    {
        winTexts = winMessage.GetComponentsInChildren<Text>();
        m_missionSystem = GameObject.Find("PersistentGameObject");

    }

    // Update is called once per frame
    void Update()
    {
        if (enemyMsgTimer > 0f)
            enemyMsgTimer -= Time.deltaTime;
        else
        {
            if(enemyMsgTimer < 0f)
                NoWarning();

            enemyMsgTimer = 0f;
        }        
    }

    void NoWarning()
    {
        GetComponent<Renderer>().material.mainTexture = HUD;
    }
    void EnemyClose()
    {
        enemyMsgTimer = 5f;
        GetComponent<Renderer>().material.mainTexture = enemyCloseImage;
    }
    void MissileIncoming()
    {
        if(enemyMsgTimer == 0f)
            GetComponent<Renderer>().material.mainTexture = missileImage;
        else
            GetComponent<Renderer>().material.mainTexture = enemyMissileComb;
    }
    void LootPickUp()
    {
        GetComponent<Renderer>().material.mainTexture = lootPickUpImage;
        m_missionSystem.SendMessage("LootPickedUp");
        StartCoroutine(LootMessageWait());
    }    

    void Win()
    {
        GetComponent<Renderer>().material.mainTexture = HUD;
        winMessage.SetActive(true);
        Debug.Log("Won");
        StartCoroutine(WinMessage());
    }

    IEnumerator LootMessageWait()
    {
        yield return new WaitForSeconds(3.0f);
        GetComponent<Renderer>().material.mainTexture = HUD;
    }

    IEnumerator WinMessage()
    {
        winTexts[2].text = "Returning to Main Menu .";
        yield return new WaitForSeconds(1.0f);
        winTexts[2].text = "Returning to Main Menu . .";
        yield return new WaitForSeconds(1.0f);
        winTexts[2].text = "Returning to Main Menu . . .";
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("MainMenu");
    }
}
