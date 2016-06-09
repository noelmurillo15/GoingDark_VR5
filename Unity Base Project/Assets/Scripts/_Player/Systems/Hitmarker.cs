using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Hitmarker : MonoBehaviour
{
    [SerializeField]
    private Sprite StaticMarker;
    [SerializeField]
    private Sprite hitMarker;

    private float HitDisplayDuration = 0.8f;
    private float HitTime;
    private bool ShowHitMarker;

    // Use this for initialization
    void Start()
    {
        ShowHitMarker = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (GetComponent<Image>().sprite != StaticMarker && Time.time - HitTime > HitDisplayDuration)
        {
          //  Debug.Log("Switched to static");
            GetComponent<Image>().sprite = StaticMarker;
        }

    }

    public void HitMarkerShow(float TimeWhenShot)
    {
            //Debug.Log("Switched to Hitmarker");
        HitTime = TimeWhenShot;
        GetComponent<Image>().sprite = hitMarker;
    }


}
