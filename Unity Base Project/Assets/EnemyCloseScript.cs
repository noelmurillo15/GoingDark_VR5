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

    // Update is called once per frame
    void Update()
    {

    }

    public void SetEnemyCloseText(int NewNumber)
    {
        if (NewNumber < 2)
            EnemyCloseText.text = NewNumber.ToString() + " Enemy Close";
        else
            EnemyCloseText.text = NewNumber.ToString() + " Enemies Close";


    }
}
