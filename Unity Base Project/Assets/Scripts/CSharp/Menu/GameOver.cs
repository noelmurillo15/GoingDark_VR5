using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    PlayerStats stats;
    string currentScene;
    AsyncOperation loadOperation;
    LoadSceneMode loadSceneMode = LoadSceneMode.Additive;

    // Use this for initialization
    void Start()
    {
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        currentScene = SceneManager.GetActiveScene().name;
    }

    public void InitializeGameOverScene()
    {
        StartCoroutine(LoadAsync("GameOver"));
    }

    private IEnumerator LoadAsync(string levelName)
    {
        BeginLoad();

        while (!FinishedLoad())
        {
            yield return null;
        }        
    }

    void BeginLoad()
    {
        loadOperation = SceneManager.LoadSceneAsync("GameOver", loadSceneMode);

        GameObject[] sceneobjs = SceneManager.GetSceneByName(currentScene).GetRootGameObjects();
        for (int x = 0; x < sceneobjs.Length; x++)
            sceneobjs[x].SetActive(false);
    }

    bool FinishedLoad()
    {
        return loadOperation.isDone;
    }
}
