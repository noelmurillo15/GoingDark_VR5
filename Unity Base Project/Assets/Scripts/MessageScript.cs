using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MessageScript : MonoBehaviour {


    [SerializeField]
    Text missile;
    [SerializeField]
    Text enemyClose;
    [SerializeField]
    Text lootPickUp;
    [SerializeField]
    GameObject winMessage;

    private Text[] winTexts;

    // Use this for initialization
    void Start () {
        winTexts = winMessage.GetComponentsInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void EnemyClose()
    {
        enemyClose.gameObject.SetActive(true);
    }

    void EnemyAway()
    {
        enemyClose.gameObject.SetActive(false);

    }

    void LootPickUp()
    {
        lootPickUp.gameObject.SetActive(true);
        StartCoroutine(LootMessageWait());
    }

    void MissileIncoming()
    {
        missile.gameObject.SetActive(true);
        Debug.Log("Incoming Missile");
    }

    void MissileDestroyed()
    {
        missile.gameObject.SetActive(false);
        Debug.Log("Missile Destroyed");

    }

    void Win()
    {
        winMessage.SetActive(true);
        Debug.Log("Won");
        StartCoroutine(WinMessage());
    }

    IEnumerator LootMessageWait()
    {
        yield return new WaitForSeconds(3.0f);
        lootPickUp.gameObject.SetActive(false);
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
