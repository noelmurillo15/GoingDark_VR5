using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{

    [SerializeField]
    private GameObject MainMenuPanel;
    [SerializeField]
    private GameObject GamePanel;
    [SerializeField]
    private GameObject SettingsPanel;
    [SerializeField]
    private GameObject VideoPanel;
    [SerializeField]
    private GameObject AudioPanel;
    [SerializeField]
    private GameObject ControlsPanel;
    [SerializeField]
    private GameObject LoadPanel;
    [SerializeField]
    private GameObject NamePanel;

    private LoadGame LoadGame;

    // Use this for initialization
    void Start()
    {
        LoadGame = gameObject.GetComponent<LoadGame>();

    }

    public void LoadScene(string scenename)
    {
        SceneManager.UnloadScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(scenename);
    }

    public void Options()
    {
        MainMenuPanel.SetActive(false);
        ControlsPanel.SetActive(false);
        SettingsPanel.SetActive(true);
        GamePanel.SetActive(false);
        VideoPanel.SetActive(false);
        AudioPanel.SetActive(false);
    }

    public void MainMenu()
    {
        LoadPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }

    public void Game()
    {
        MainMenuPanel.SetActive(false);
        GamePanel.SetActive(true);
    }
    public void ChooseName()
    {

    }
    public void Load()
    {
        MainMenuPanel.SetActive(false);
        LoadPanel.SetActive(true);
    }

    public void Video()
    {
        SettingsPanel.SetActive(false);
        VideoPanel.SetActive(true);
    }

    public void Audio()
    {
        SettingsPanel.SetActive(false);
        AudioPanel.SetActive(true);
    }

    public void Controls()
    {
        SettingsPanel.SetActive(false);
        ControlsPanel.SetActive(true);
    }

    public void LoadSlot(string slotName)
    {
        LoadGame.Load(slotName);
        SceneManager.UnloadScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("LevelSelect");
    }

    public void Quit()
    {
#if UNITY_EDITOR
        SceneManager.UnloadScene(SceneManager.GetActiveScene().name);
        UnityEditor.EditorApplication.isPlaying = false;
#else
        SceneManager.UnloadScene(SceneManager.GetActiveScene().name);
        Application.Quit();
#endif
    }


    #region Settings
    public void ChangeDifficulty(string diff)
    {
        PlayerPrefs.SetString("Difficulty", diff);
        GamePanel.SetActive(false);
        NamePanel.SetActive(true);
    }
    #endregion
}
