using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PauseManager : MonoBehaviour
{

    #region Properties
    //  Panel Info
    [SerializeField]
    private bool paused;
    [SerializeField]
    private GameObject MainPanel;
    [SerializeField]
    private GameObject TitleTexts;
    [SerializeField]
    private GameObject HowToPanel;

    //  Menu Scene Name
    public string mainMenu = "MainMenu";
    private string currLevel;

    //  Time-Scale
    public float timeScale = 1f;

    //  Settings Values
    [SerializeField]
    protected int msaaINI;
    [SerializeField]
    protected int vsyncINI;
    [SerializeField]
    protected float aaQualINI;
    [SerializeField]
    protected float renderDistINI;

    //  Resoultions
    private bool isFullscreen;
    private Resolution currentRes;

    // for saving
    private SaveGame saveGame;
    private PersistentGameManager gameManager;

    //  Player Data
    private PlayerStats stats;
    private PlayerInput playerInput;

    //  Controller
    private x360Controller control;
    #endregion


    public void Start()
    {
        paused = false;
        currLevel = SceneManager.GetActiveScene().name;
        gameManager = PersistentGameManager.Instance;

        saveGame = gameObject.GetComponent<SaveGame>();
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        stats = playerInput.GetComponent<PlayerStats>();

        //  Get the current resoultion
        currentRes = Screen.currentResolution;
        isFullscreen = Screen.fullScreen;

        //  Get all int values
        vsyncINI = QualitySettings.vSyncCount;
        msaaINI = QualitySettings.antiAliasing;
        aaQualINI = QualitySettings.antiAliasing;

        //  Enable titles
        TitleTexts.SetActive(true);
        MainPanel.SetActive(false);
        HowToPanel.SetActive(false);

        //  Access Game Controller
        control = GamePadManager.Instance.GetController(0);
    }

    public void Update()
    {
        if (control.GetButtonDown("Start"))
        {
            Pause(!paused);
        }
    }

    public void Pause(bool boolean)
    {
        paused = boolean;
        if (paused)
            Time.timeScale = 0f;
        else
            Time.timeScale = timeScale;

        HowToPanel.SetActive(false);
        MainPanel.SetActive(paused);
        TitleTexts.SetActive(paused);
        playerInput.MessageUp(paused);
    }

    #region Button Events
    public void HowToPlayMenu()
    {
        MainPanel.SetActive(false);
        HowToPanel.SetActive(true);
    }
    public void BackFromHowToPlay()
    {
        MainPanel.SetActive(true);
        HowToPanel.SetActive(false);
    }
    public void Restart()
    {
        Time.timeScale = timeScale;
        SceneManager.LoadScene(currLevel);
    }
    public void ReturnToMainMenu()
    {
        AutoSave();
        Time.timeScale = timeScale;
        SceneManager.LoadScene(mainMenu);
    }
    public void quitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    #endregion

    #region Saving
    public void AutoSave()
    {
        gameManager.SetPlayerCredits(stats.GetCredits());
        saveGame.Save(gameManager.GetSaveSlot());
    }
    #endregion

    #region V-Sync
    public void toggleVSync(bool _bool)
    {
        if (_bool)
            QualitySettings.vSyncCount = 1;
        else
            QualitySettings.vSyncCount = 0;

        vsyncINI = QualitySettings.vSyncCount;
    }
    #endregion

    #region Screen Resolution
    public void setFullScreen(bool _bool)
    {
        isFullscreen = _bool;
        if (isFullscreen)
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        else
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, false);
    }
    #endregion

    #region Multi-Sampling Anti-Alaising
    public void setMSAA(int msaaAmount)
    {
        if (msaaAmount == 0)
        {
            disableMSAA();
        }
        else if (msaaAmount == 1)
        {
            twoMSAA();
        }
        else if (msaaAmount == 2)
        {
            fourMSAA();
        }
        else if (msaaAmount == 3)
        {
            eightMSAA();
        }
    }
    public void disableMSAA()
    {
        QualitySettings.antiAliasing = 0;
    }
    public void twoMSAA()
    {
        QualitySettings.antiAliasing = 2;
    }
    public void fourMSAA()
    {
        QualitySettings.antiAliasing = 4;
    }
    public void eightMSAA()
    {
        QualitySettings.antiAliasing = 8;
    }
    #endregion

    #region Graphics Quality
    public void setFastest()
    {
        QualitySettings.SetQualityLevel(0);
    }
    public void setFast()
    {
        QualitySettings.SetQualityLevel(1);
    }
    public void setSimple()
    {
        QualitySettings.SetQualityLevel(2);
    }
    public void setGood()
    {
        QualitySettings.SetQualityLevel(3);
    }
    public void setBeautiful()
    {
        QualitySettings.SetQualityLevel(4);
    }
    public void setFantastic()
    {
        QualitySettings.SetQualityLevel(5);
    }
    #endregion
}