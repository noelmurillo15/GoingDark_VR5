using UnityEngine;
using System.Collections;

public class Hitmarker : MonoBehaviour
{
    [SerializeField]
    private Sprite StaticMarker;
    [SerializeField]
    private Sprite hitMarker;

    private Transform MyTransform;
    private SpriteRenderer srend;
    private RaycastHit hit;
    private int range;

    private float HitDisplayDuration = 0.5f;
    private float HitTime;
    private bool ShowHitMarker;

    // Use this for initialization
    void Start()
    {
        range = 1600;
        MyTransform = transform;
        srend = GetComponent<SpriteRenderer>();
        ShowHitMarker = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (srend.sprite != StaticMarker && Time.time - HitTime > HitDisplayDuration)
            srend.sprite = StaticMarker;
        
        if (Physics.Raycast(MyTransform.position, MyTransform.forward, out hit, range))
        {
            if (hit.collider.CompareTag("Enemy") && hit.collider.GetType() == typeof(BoxCollider))
            {
                srend.color = Color.red;
                return;
            }
            if (hit.collider.CompareTag("Asteroid"))
            {
                srend.color = Color.red;
                return;
            }            
        }
        srend.color = Color.white;
    }

    public void HitMarkerShow(float TimeWhenShot)
    {
        HitTime = TimeWhenShot;
        srend.sprite = hitMarker;
    }
}
