using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeName : MonoBehaviour {

    [SerializeField]
    private Text Name;
    private PersistentGameManager GameManager;
	// Use this for initialization
	void Start () {
        GameManager = PersistentGameManager.Instance;
	}
	
	public void PrintLetter(string letter)
    {   if (Name.text == "NAME")
            Name.text = "";
        Debug.Log("Length : "  + Name.text.Length + Name.text);
        if (Name.text.Length < 14)
            Name.text += letter;
    }

    public void Done()
    {
        GameManager.SetPlayerName(Name.text);
        SceneManager.LoadScene("LevelSelect");
    }
}
