using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour {

    private float transition;
    private float cancelTimer;
    private Image image;
    private Color originalColor;
    private Button button;
    // Use this for initialization
    void Start () {
        transition = 0f;
        cancelTimer = 0f;
        button = GetComponent<Button>();
        image = transform.GetChild(0).GetComponent<Image>(); ;
        originalColor = new Color(0f, 0.549f,1f,1f);
    }

    // Update is called once per frame
    void Update () {
    }

    private void ActivateButton()
    {
        AudioManager.instance.PlayMenuGood();

        button.onClick.Invoke();
        image.color = originalColor;
    }

    #region Collision
    public void OnTriggerEnter(Collider col)
    {
        if (col.name == "bone3")
        {
            transition = 0.1f;
            cancelTimer = 1.5f;
            image.color = Color.white;
            Debug.Log("asf");
        }
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.name == "bone3")
        {
            if (cancelTimer > 0.0f)
            {
                transition -= Time.deltaTime;
                cancelTimer -= Time.deltaTime;

                if (transition <= 0.0f && image.color == Color.white)
                    image.color = Color.green;
            }
            else
                image.color = Color.red;
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.name == "bone3")
            if (image.color == Color.green)
                ActivateButton();
            else
                image.color = originalColor;
    }
    #endregion    
}
