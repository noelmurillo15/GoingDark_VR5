using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Hitmarker : MonoBehaviour
{
    [SerializeField]
    private Sprite StaticMarker;
    [SerializeField]
    private Sprite hitMarker;

    private Image reticle;

    private float HitDisplayDuration = 0.8f;
    private float HitTime;
    private bool ShowHitMarker;

    private int range;
    private bool rayhit;
    private RaycastHit hit;
    private Transform MyTransform;

    // Use this for initialization
    void Start()
    {
        range = 1600;
        rayhit = false;
        ShowHitMarker = false;
        reticle = GetComponent<Image>();
        MyTransform = transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        rayhit = false;
        if (GetComponent<Image>().sprite != StaticMarker && Time.time - HitTime > HitDisplayDuration)
        {
            //  Debug.Log("Switched to static");
            GetComponent<Image>().sprite = StaticMarker;
        }
        if (Physics.Raycast(MyTransform.position, MyTransform.forward, out hit, range))
        {
            if (hit.collider.CompareTag("Asteroid"))
                rayhit = true;
            else if (hit.collider.CompareTag("Enemy") && hit.collider.GetType() == typeof(BoxCollider))
                rayhit = true;
            else if (hit.collider.CompareTag("Station"))
                rayhit = true;
        }

        if (rayhit)
            reticle.color = Color.red;
        else
            reticle.color = Color.white;
    }

    public void HitMarkerShow(float TimeWhenShot)
    {
            //Debug.Log("Switched to Hitmarker");
        HitTime = TimeWhenShot;
        GetComponent<Image>().sprite = hitMarker;
    }


}
