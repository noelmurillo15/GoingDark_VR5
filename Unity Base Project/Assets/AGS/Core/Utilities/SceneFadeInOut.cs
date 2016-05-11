using AGS.Core.Classes.TimerComponents;
using AGS.Core.Systems.GameLevelSystem;
using UnityEngine;

namespace AGS.Core.Utilities
{
    /// <summary>
    /// Original Script by Unity3D Stealth Tutorial
    /// Modified with extra start delay. Place this on GameView object
    /// </summary>
    public class SceneFadeInOut : MonoBehaviour
    {
        
        public float FadeSpeed = 1.5f;          // Speed that the screen fades to and from black.
        private bool _sceneStarting;      // Whether or not the scene is still fading in.
        private float _delay;

        void Awake()
        {
            // Set the texture so that it is the the size of the screen and covers it.
            GetComponent<GUITexture>().pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
            _delay = GetComponent<GameLevelBaseView>().StartDelaySeconds + 1f;
        }

        void Start()
        {
            var levelStartTimer = Classes.MonoExtensions.ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject, "Start timer");
            levelStartTimer.TimerMethod = () => _sceneStarting = true;
            levelStartTimer.Invoke(_delay);
        }

        void Update()
        {
            // If the scene is starting...
            if (_sceneStarting)
                // ... call the StartScene function.
                StartScene();
        }


        void FadeToClear()
        {
            // Lerp the colour of the texture between itself and transparent.
            GetComponent<GUITexture>().color = Color.Lerp(GetComponent<GUITexture>().color, Color.clear, FadeSpeed * Time.deltaTime);
        }


        void FadeToBlack()
        {
            // Lerp the colour of the texture between itself and black.
            GetComponent<GUITexture>().color = Color.Lerp(GetComponent<GUITexture>().color, Color.black, FadeSpeed * Time.deltaTime);
        }


        void StartScene()
        {
            // Fade the texture to clear.
            FadeToClear();

            // If the texture is almost clear...
            if (GetComponent<GUITexture>().color.a <= 0.05f)
            {
                // ... set the colour to clear and disable the GUITexture.
                GetComponent<GUITexture>().color = Color.clear;
                GetComponent<GUITexture>().enabled = false;

                // The scene is no longer starting.
                _sceneStarting = false;
            }
        }


        public void EndScene()
        {
            // Make sure the texture is enabled.
            GetComponent<GUITexture>().enabled = true;

            // Start fading towards black.
            FadeToBlack();

            // If the screen is almost black...
            if (GetComponent<GUITexture>().color.a >= 0.95f)
                // ... reload the level.
                Application.LoadLevel(0);
        }

        public void Pause()
        {
            GetComponent<GUITexture>().enabled = true;
            var color = GetComponent<GUITexture>().color;
            GetComponent<GUITexture>().color = new Color(color.r, color.g, color.b, 0.25f);
        }
        public void Resume()
        {
            var color = GetComponent<GUITexture>().color;
            GetComponent<GUITexture>().color = new Color(color.r, color.g, color.b, 1f);
            GetComponent<GUITexture>().enabled = false;
        }
    }
}