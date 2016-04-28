using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour {


    [SerializeField]
    Text mText;
    [SerializeField]
    Text skipText;
    [SerializeField]
    Image hands;

    private LeapData m_leapData;

    private float timer;
    private bool skipActivate;
	// Use this for initialization
	void Start () {
        timer = 5.0f;
        skipActivate = false;

        m_leapData = GameObject.FindGameObjectWithTag("LeapControl").GetComponent<LeapData>();
	}
	
	// Update is called once per frame
	void Update () {

        if (m_leapData.GetNumHands() == 2 && skipActivate)
            SceneManager.LoadScene("MainMenu");

        timer -= Time.deltaTime;
        if (timer <= 0.0f)
            skipActivate = true;

        if (skipActivate && !skipText.IsActive())
        {
            skipText.gameObject.SetActive(true);
            hands.gameObject.SetActive(true);

            //if (!skipText.IsActive())
            //{
            //   
            //}

            //if (skipText.material.color.a == 1.0f)
            //    skipText.CrossFadeAlpha(0.2f, 1.0f, false);
            //else if (skipText.material.color.a == 0.2f)
            //    skipText.CrossFadeAlpha(1.0f, 1.0f, false); 
            //FadeOut();
        }
        mText.transform.Translate(Vector3.up * Time.deltaTime * 25);
    }

    IEnumerator FadeIn()
    {
       skipText.CrossFadeAlpha(1.0f, 0.5f, false);
       yield return new WaitForSeconds(2.0f);
       FadeOut();
    }

    IEnumerator FadeOut()
    {
        skipText.CrossFadeAlpha(0.2f, 0.5f, false);
        yield return new WaitForSeconds(2.0f);
    }
}
