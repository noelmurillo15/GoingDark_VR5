using UnityEngine;
using System.Collections;

public class ScreenBreak : MonoBehaviour {

    private Texture2D screenBreak;
    private Texture2D shatter;
    float fadeTimer;
    // Use this for initialization
    void Start () {
        screenBreak = null;
        shatter = Resources.Load<Texture2D>("broken-glass");
        fadeTimer = 1.0f;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (screenBreak)
        {
            fadeTimer -= Time.deltaTime * 0.15f;
            GUI.color = new Color(1, 1, 1, fadeTimer);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), screenBreak, ScaleMode.StretchToFill);
            if (GUI.color.a <= 0)
            {
                GUI.color = new Color(1, 1, 1, 1);
                screenBreak = null;
                fadeTimer = 1.0f;
            }
        }
    }

    public void Shatter()
    {
        screenBreak = shatter;
        GUI.color = new Color(1, 1, 1, 1);
        fadeTimer = 1.0f;
    }
}
