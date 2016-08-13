﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace GoingDark.Core.Utility
{

    public class PauseManager : MonoBehaviour
    {

        #region Properties
        //  Main Camera
        public Camera mainCam;

        //  Panel Info
        public bool paused;
        public Text pauseMenu;
        public GameObject mainPanel;
        public GameObject TitleTexts;
        public GameObject HTP;
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
        protected float fovINI;
        [SerializeField]
        protected float aaQualINI;
        [SerializeField]
        protected float shadowDistINI;
        [SerializeField]
        protected float renderDistINI;

        //  Resoultions
        private bool isFullscreen;
        private Resolution currentRes;

        //  Controller
        private x360Controller control;
        // for saving
        private SaveGame saveGame;
        private LoadGame loadGame;
        [SerializeField]
        private GameObject saveSlots;
        [SerializeField]
        private GameObject[] Slots;

        private PlayerInput playerInput;
        private PlayerStats stats;
        #endregion


        public void Start()
        {
            paused = false;
            playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
            stats = playerInput.GetComponent<PlayerStats>();

            currLevel = SceneManager.GetActiveScene().name;

            saveGame = gameObject.GetComponent<SaveGame>();
            loadGame = gameObject.GetComponent<LoadGame>();
            //  Get the current resoultion
            currentRes = Screen.currentResolution;
            isFullscreen = Screen.fullScreen;

            //  Get all int values
            fovINI = mainCam.fieldOfView;
            renderDistINI = mainCam.farClipPlane;
            vsyncINI = QualitySettings.vSyncCount;
            msaaINI = QualitySettings.antiAliasing;
            aaQualINI = QualitySettings.antiAliasing;
            shadowDistINI = QualitySettings.shadowDistance;

            //  Enable titles
            TitleTexts.SetActive(true);
            //  Disable other panels
            mainPanel.SetActive(false);
            HTP.SetActive(false);

            //  Access Game Controller
            control = GamePadManager.Instance.GetController(0);
        }

        public void Update()
        {
            if (mainPanel.activeSelf == true)
            {
                pauseMenu.text = "Pause Menu";
            }
            if (control.GetButtonDown("Back"))
            {
                if (!paused)
                {
                    playerInput.MessageUp(true);
                    paused = true;
                    mainPanel.SetActive(true);
                    TitleTexts.SetActive(true);
                    Time.timeScale = 0;
                }
                else
                {
                    playerInput.MessageUp(false);
                    Resume();
                }
            }

            if (paused)
            {

            }
        }

        public void Resume()
        {
            paused = false;
            Time.timeScale = timeScale;
            mainPanel.SetActive(false);
            HTP.SetActive(false);
            TitleTexts.SetActive(false);
        }
        public void Restart()
        {
            if (Time.timeScale == 0f)
                Time.timeScale = timeScale;

            SceneManager.UnloadScene(currLevel);
            SceneManager.LoadScene(currLevel);
        }
        public void returnToMenu()
        {
            if (Time.timeScale == 0f)
                Time.timeScale = timeScale;

            SceneManager.UnloadScene(currLevel);
            SceneManager.LoadScene(mainMenu);
        }
        public void quitGame()
        {
#if UNITY_EDITOR
            SceneManager.UnloadScene(currLevel);
            UnityEditor.EditorApplication.isPlaying = false;
#else
            SceneManager.UnloadScene(currLevel);
            Application.Quit();
#endif
        }

        public void SaveGame()
        {
            CheckSlots();
            Debug.Log("Saving the game");
            PlayerPrefs.SetInt("Credits", stats.GetCredits());            
            saveSlots.SetActive(true);
            mainPanel.SetActive(false);
        }
        void CheckSlots()
        {
            string name = string.Empty;
            for (int i = 0; i < Slots.Length; i++)
            {
                name = loadGame.IsSlotUsed(Slots[i].name);
                if (name != "Name")
                    Slots[i].GetComponentInChildren<Text>().text = name;
                else
                    Slots[i].GetComponentInChildren<Text>().text = Slots[i].GetComponentInChildren<Text>().text;
            }

        }
        public void SaveAtSlot(string txt)
        {
            saveGame.Save(txt);
            saveSlots.SetActive(false);
            mainPanel.SetActive(true);
        }

        public void ToPauseMenu()
        {
            saveSlots.SetActive(false);
            HTP.SetActive(false);
            mainPanel.SetActive(true);
        }

        public void HowToPlayMenu()
        {
            mainPanel.SetActive(false);
            HTP.SetActive(true);
        }

        public void BackFromHowToPlay()
        {
            mainPanel.SetActive(true);
            HTP.SetActive(false);
        }

        public void toggleVSync(bool B)
        {
            vsyncINI = QualitySettings.vSyncCount;
            if (B == true)
            {
                QualitySettings.vSyncCount = 1;
            }
            else
            {
                QualitySettings.vSyncCount = 0;
            }
        }


        public void updateFOV(float fov)
        {
            mainCam.fieldOfView = 106f;
        }


        // Full Screen
        public void setFullScreen(bool b)
        {
            if (b == true)
            {
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
                isFullscreen = true;
            }
            else
            {
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, false);
                isFullscreen = false;
            }
        }


        #region Graphics Settings
        //  MSAA
        public void updateMSAA(int msaaAmount)
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

        //  Graphics Quality
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
}
