using UnityEngine;
using AssetBundles;
using System.Collections;
#if !(UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2)
using UnityEngine.SceneManagement;
#endif

namespace Gd.Core.Utilities
{
    public class OpenWorldStreamer : MonoBehaviour
    {
        #region Properties
        public int maxScenes = 10;
        public string currentSceneName;
        public string currentAssetBundle;
        public static OpenWorldStreamer _instance = null;
        #endregion

        #region OpenWorld Streamer
        void Awake()
        {
            if (_instance)
                Destroy(this);
            else
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }            
        }

        IEnumerator Start()
        {
            yield return StartCoroutine(Initialize());

            Debug.Log("Loading all levels");
            for (int x = 1; x < maxScenes; x++)
            {
                string sceneName = "Scene ";
                sceneName += x.ToString();
                yield return StartCoroutine(InitializeLevelAsync(sceneName, true));
            }
        }

        protected IEnumerator Initialize()
        {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
            AssetBundleManager.SetDevelopmentAssetBundleServer();
#else
        		// Use the following code if AssetBundles are embedded in the project for example via StreamingAssets folder etc:
        		AssetBundleManager.SetSourceAssetBundleURL(Application.dataPath + "/");
        		// Or customize the URL based on your deployment or configuration
        		//AssetBundleManager.SetSourceAssetBundleURL("http://www.MyWebsite/MyAssetBundles");
#endif

            // Initialize AssetBundleManifest which loads the AssetBundleManifest object.
            var request = AssetBundleManager.Initialize();

            if (request != null)
                yield return StartCoroutine(request);
        }

        protected IEnumerator InitializeLevelAsync(string levelName, bool isAdditive)
        {
            // This is simply to get the elapsed time for this phase of AssetLoading.
            //float startTime = Time.realtimeSinceStartup;

            // Load level from assetBundle.
            AssetBundleLoadOperation request = AssetBundleManager.LoadLevelAsync(currentAssetBundle, levelName, isAdditive);
            if (request == null)
                yield break;
            yield return StartCoroutine(request);

            // Calculate and display the elapsed time.
            //float elapsedTime = Time.realtimeSinceStartup - startTime;
            //Debug.Log("Finished loading scene " + levelName + " in " + elapsedTime + " seconds");
        }

        private void DestroyScene()
        {
            float startTime = Time.realtimeSinceStartup;
            SceneManager.UnloadScene(currentSceneName);
            AssetBundleManager.UnloadAssetBundle(currentAssetBundle);
            float elapsedTime = Time.realtimeSinceStartup - startTime;
            Debug.Log("Finished unloading scene " + currentSceneName + " in " + elapsedTime + " seconds");
        }
        #endregion
    }
}
