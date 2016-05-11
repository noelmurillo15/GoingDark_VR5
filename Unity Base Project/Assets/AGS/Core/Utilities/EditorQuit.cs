using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace AGS.Core.Utilities
{
    /// <summary>
    /// To be able to quit play mode in editor
    /// </summary>
    public class EditorQuit : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
