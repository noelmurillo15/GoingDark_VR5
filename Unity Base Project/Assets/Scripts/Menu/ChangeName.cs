using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeName : MonoBehaviour
{

    [SerializeField]
    private Text Name;
    private Mainmenu MainMenu;

    private PersistentGameManager GameManager;
    // Use this for initialization
    void Start()
    {
        GameManager = PersistentGameManager.Instance;
        MainMenu = gameObject.GetComponent<Mainmenu>();
    }

    public string GetName() { return Name.text; }

    public void PrintLetter(string letter)
    {
        if (Name.text == "NAME")
            Name.text = "";
        if (Name.text.Length < 14)
            Name.text += letter;
    }

    public void Done()
    {
        GameManager.SetPlayerName(Name.text);
        MainMenu.OpenNewSave();
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
