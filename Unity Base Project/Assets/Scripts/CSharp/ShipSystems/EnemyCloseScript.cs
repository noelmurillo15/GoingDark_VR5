using UnityEngine;
using UnityEngine.UI;


public class EnemyCloseScript : MonoBehaviour
{
    private Text EnemyCloseText;


    // Use this for initialization
    void Start()
    {
        EnemyCloseText = transform.GetComponent<Text>();
    }


    public void SetEnemyCloseText(int NewNumber)
    {
        if (NewNumber < 2)
            EnemyCloseText.text = "1 Enemy Close";
        else
            EnemyCloseText.text = NewNumber.ToString() + " Enemies Close";
    }
}
