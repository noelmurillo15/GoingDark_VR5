using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeName : MonoBehaviour
{

    [SerializeField]
    private Text Name;
    private PersistentGameManager GameManager;
    private string lastChar;
    // Use this for initialization
    void Start()
    {
        GameManager = PersistentGameManager.Instance;
    }

    public void PrintLetter(string letter)
    {
        if (Name.text == "NAME")
            Name.text = "";
        if (Name.text.Length < 14)
            Name.text += letter;

        lastChar = letter;
    }

    public void Done()
    {
        GameManager.SetPlayerName(Name.text);
        SceneManager.LoadScene("LevelSelect");
    }

    public void DeleteLetter()
    {
        if (Name.text.Length > 0)
        {
            string temp = Name.text;
            temp = temp.Remove(temp.Length - 1);
            Name.text = temp;
        }
    }
}
