using UnityEngine;
using UnityEngine.UI;

public class Hitmarker : MonoBehaviour
{
    [SerializeField]
    private Sprite StaticMarker;
    [SerializeField]
    private Sprite hitMarker;

    private Image reticle;
    private float HitDisplayDuration;
    private float HitTime;
    private bool ShowHitMarker;

    private int range;
    private int layermask;
    private bool rayhit;
    private RaycastHit hit;
    private Transform MyTransform;

    private GameObject TargetImg;
    private GameObject LockOnMarker;
    private bool flip;
    // Use this for initialization
    void Start()
    {
        range = 1500;
        layermask = 1 << 11;    //  enemies layer
        rayhit = true;
        ShowHitMarker = false;
        reticle = GetComponent<Image>();
        MyTransform = transform;
        HitDisplayDuration = 0.8f;
        TargetImg = Resources.Load<GameObject>("LockObject");
        LockOnMarker = Instantiate(TargetImg, Vector3.zero, Quaternion.identity) as GameObject;
        LockOnMarker.SetActive(false);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        rayhit = false;
        if (GetComponent<Image>().sprite != StaticMarker && Time.time - HitTime > HitDisplayDuration)
            GetComponent<Image>().sprite = StaticMarker;

        if (Physics.Raycast(MyTransform.position, MyTransform.forward, out hit, range, layermask))
        {          
            if (hit.collider.CompareTag("Enemy") && hit.collider.GetType() == typeof(BoxCollider)) { 
                rayhit = true;
                Invoke("StartTimer", .5f);
            }        
        }

        Color color = rayhit ? Color.green : Color.red;
        Debug.DrawRay(MyTransform.position, MyTransform.forward * range, color);

        if (rayhit && flip)
        {
            if (LockOnMarker != null)
                LockOnMarker.SetActive(true);
            objUpdate();
            reticle.color = Color.red;
        }
        else if (rayhit)
        {
            reticle.color = Color.red;
            flip = false;
        }
        else
        {
            CancelInvoke();
            flip = false;
            reticle.color = Color.white;
            if(LockOnMarker != null)
                LockOnMarker.SetActive(false);
        }

    }
    bool StartTimer()
    {
        if (rayhit)
            flip = true;
        if (!rayhit)
            flip = false;
        return false;
    }

    public void HitMarkerShow(float TimeWhenShot)
    {
        HitTime = TimeWhenShot;
        GetComponent<Image>().sprite = hitMarker;
    }
    void objUpdate()
    {
        if (LockOnMarker != null)
        {
            LockOnMarker.transform.parent = hit.transform;
            LockOnMarker.transform.position = hit.transform.position;
            LockOnMarker.transform.LookAt(MyTransform);
        }

        if (LockOnMarker == null)
        {
            LockOnMarker = Instantiate(TargetImg, Vector3.zero, Quaternion.identity) as GameObject;
        }
    }
}