using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartGame()
    {
        //SceneManager.LoadScene("LevelSelect");
        LoadingScreenManager.LoadScene("LevelSelect");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
