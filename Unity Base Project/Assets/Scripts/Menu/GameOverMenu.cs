using UnityEngine;
using UnityEngine.UI;
using GoingDark.Core.Enums;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{

    #region Properties
    [SerializeField]
    private Button Continue;

    [SerializeField]
    private Text m_guiText;

    private int money;
    private int respawnCost;
    private Scene[] all_scenes;
    private GameDifficulty diff;
    private const string display = "Credits Owned : {0}  Credits Needed : {1}";
    #endregion


    void Start()
    {
        respawnCost = 0;
        money = PlayerPrefs.GetInt("Credits");
        switch (PlayerPrefs.GetString("Difficulty"))
        {
            case "Easy":
                respawnCost = 200;
                break;
            case "Medium":
                respawnCost = 200 * 2;
                break;
            case "Hard":
                respawnCost = 200 * 3;
                break;
            case "Nightmare":
                respawnCost = 200 * 5;
                break;
        }
        m_guiText.text = string.Format(display, money, respawnCost);
        if (money < respawnCost)
        {
            Continue.FindSelectableOnDown().Select();
            Continue.interactable = false;
        }
    }

    public void Respawn()
    {
        if(money < respawnCost)
            return;
                
        //  Unload GameOver
        SceneManager.UnloadScene("GameOver");

        //  Get The Level Still Active
        string scenename = SceneManager.GetActiveScene().name;
        GameObject[] sceneobjs = SceneManager.GetSceneByName(scenename).GetRootGameObjects();

        //  Enable all its Gameobjects
        for (int x = 0; x < sceneobjs.Length; x++)
            sceneobjs[x].SetActive(true);        

        //  Respawn The Player
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().Respawn();
    }

    public void LoadScene(string scenename)
    {
        //for (int x = 0; x < SceneManager.sceneCount; x++)
        //    SceneManager.UnloadScene(SceneManager.GetSceneAt(x).name);
        
        SceneManager.LoadScene(scenename);
    }
}