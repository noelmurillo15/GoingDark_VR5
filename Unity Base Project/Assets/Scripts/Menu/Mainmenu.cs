﻿using UnityEngine;
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
    [SerializeField]
    private GameObject NewGamePanel;

    private LoadGame LoadGame;
    private SaveGame SaveGame;
    private PersistentGameManager gameManager;
    // Use this for initialization
    void Start()
    {
        LoadGame = gameObject.GetComponent<LoadGame>();
        SaveGame = gameObject.GetComponent<SaveGame>();

        gameManager = PersistentGameManager.Instance;
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
        GamePanel.SetActive(false);
        VideoPanel.SetActive(false);
        AudioPanel.SetActive(false);
    }

    public void MainMenu()
    {
        LoadPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
        GamePanel.SetActive(false);
    }

    public void Game()
    {
        MainMenuPanel.SetActive(false);
        GamePanel.SetActive(true);
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
        gameManager.SetSaveSlot(slotName);
        SceneManager.LoadScene("LevelSelect");
    }

    public void NewGame(string slotName)
    {
        SaveGame.Save(slotName);
        gameManager.SetSaveSlot(slotName);
        SceneManager.LoadScene("LevelSelect");
    }

    public void OpenNewSave()
    {
        NamePanel.SetActive(false);
        NewGamePanel.SetActive(true);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    #region Settings
    public void ChangeDifficulty(string diff)
    {
        gameManager.SetDifficulty(diff);
        GamePanel.SetActive(false);
        NamePanel.SetActive(true);
    }
    #endregion
}
