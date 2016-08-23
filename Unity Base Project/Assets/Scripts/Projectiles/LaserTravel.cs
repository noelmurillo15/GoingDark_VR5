using UnityEngine;

public class LaserTravel : MonoBehaviour {

    #region Properties
    [SerializeField]
    private float speed;
    [SerializeField]
    private float delay;
    [SerializeField]
    private Vector3 travelDir;

    private float delaycount;
    private Transform MyTransform;
    #endregion

    // Use this for initialization
    void OnEnable () {
        delaycount = delay;
        MyTransform = transform;
        MyTransform.localPosition = new Vector3(0f, 0f, 0f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (delaycount <= 0f)
            MyTransform.position += travelDir * Time.fixedDeltaTime * speed;
        else
            delaycount -= Time.fixedDeltaTime;
    }
}
