using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour {

    [SerializeField]
    private GameObject MainMenuPanel;
    [SerializeField]
    private GameObject SettingsPanel;
    [SerializeField]
    private GameObject VideoPanel;
    [SerializeField]
    private GameObject AudioPanel;
    [SerializeField]
    private GameObject ControlsPanel;

    // Use this for initialization
    void Start () {
        Application.backgroundLoadingPriority = ThreadPriority.High;
    }

    public void LoadScene(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }

    public void Options()
    {
        MainMenuPanel.SetActive(false);
        ControlsPanel.SetActive(false);
        SettingsPanel.SetActive(true);
        VideoPanel.SetActive(false);
        AudioPanel.SetActive(false);
    }    

    public void MainMenu()
    {
        SettingsPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
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

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
