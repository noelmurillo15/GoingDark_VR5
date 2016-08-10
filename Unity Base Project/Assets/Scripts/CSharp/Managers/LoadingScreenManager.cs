﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{

    [Header("Loading Visuals")]
    public Image loadingIcon;
    public Image loadingDoneIcon;
    public Text loadingText;
    public Image progressBar;
    public Image fadeOverlay;

    [Header("Timing Settings")]
    public float waitOnLoadEnd = 0.25f;
    public float fadeDuration = 0.25f;

    [Header("Loading Settings")]
    public LoadSceneMode loadSceneMode = LoadSceneMode.Single;
    public ThreadPriority loadThreadPriority;

    [Header("Other")]
    // If loading additive, link to the cameras audio listener, to avoid multiple active audio listeners
    public AudioListener audioListener;

    AsyncOperation operation;
    Scene currentScene;

    public static string sceneToLoad = "MainMenu";
    // IMPORTANT! This is the build index of your loading scene. You need to change this to match your actual scene index
    static string loadingSceneName = "LoadingScene";

    public static void LoadScene(string levelName)
    {
        Application.backgroundLoadingPriority = ThreadPriority.High;
        sceneToLoad = levelName;
        SceneManager.LoadScene(loadingSceneName);
    }

    void Start()
    {
        if (sceneToLoad.Length <= 0)
            return;

        fadeOverlay.gameObject.SetActive(true); // Making sure it's on so that we can crossfade Alpha
        currentScene = SceneManager.GetActiveScene();
        StartCoroutine(LoadAsync(sceneToLoad));
    }

    private IEnumerator LoadAsync(string levelName)
    {

        StartOperation(levelName);

        float lastProgress = 0f;

        while (DoneLoading() == false)
        {
            yield return null;

            if (Mathf.Approximately(operation.progress, lastProgress) == false)
            {
                progressBar.fillAmount = operation.progress;
                lastProgress = operation.progress;
            }
        }


        if (loadSceneMode == LoadSceneMode.Additive)
            SceneManager.UnloadScene(currentScene.name);
        else
            operation.allowSceneActivation = true;
    }

    private void StartOperation(string levelName)
    {
        Application.backgroundLoadingPriority = loadThreadPriority;
        operation = SceneManager.LoadSceneAsync(levelName, loadSceneMode);


        if (loadSceneMode == LoadSceneMode.Single)
            operation.allowSceneActivation = false;
    }

    private bool DoneLoading()
    {
        return (loadSceneMode == LoadSceneMode.Additive && operation.isDone) || (loadSceneMode == LoadSceneMode.Single && operation.progress >= 0.9f);
    }

    //void FadeIn()
    //{
    //    fadeOverlay.CrossFadeAlpha(0, fadeDuration, true);
    //}

    //void FadeOut()
    //{
    //    fadeOverlay.CrossFadeAlpha(1, fadeDuration, true);
    //}
}
