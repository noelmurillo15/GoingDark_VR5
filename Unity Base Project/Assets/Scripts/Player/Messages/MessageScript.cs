using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

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
    private bool enemy;
    private bool missile;

    // Use this for initialization
    void Start()
    {
        enemy = false;
        missile = false;
        winTexts = winMessage.GetComponentsInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnemyClose()
    {
        if (missile)
            GetComponent<Renderer>().material.mainTexture = enemyMissileComb;
        else
            GetComponent<Renderer>().material.mainTexture = enemyCloseImage;
        enemy = true;
    }

    void EnemyAway()
    {
        if (missile)
            GetComponent<Renderer>().material.mainTexture = missileImage;
        else
            GetComponent<Renderer>().material.mainTexture = HUD;
        enemy = false;
    }

    void LootPickUp()
    {
        GetComponent<Renderer>().material.mainTexture = lootPickUpImage;
        StartCoroutine(LootMessageWait());
    }

    void MissileIncoming()
    {
        if (enemy)
            GetComponent<Renderer>().material.mainTexture = enemyMissileComb;
        else
            GetComponent<Renderer>().material.mainTexture = missileImage;

        missile = true;
    }

    void MissileDestroyed()
    {
        if (enemy)
            GetComponent<Renderer>().material.mainTexture = enemyCloseImage;
        else
            GetComponent<Renderer>().material.mainTexture = HUD;
        missile = false;
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
