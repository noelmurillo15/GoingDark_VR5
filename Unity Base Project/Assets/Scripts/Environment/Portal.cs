using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {
    private float jumpRangeX, jumpRangeY, jumpRangeZ;
    public Portal otherPortal;
    private float jumpTimer;
    public AudioSource teleSound;
    //private RenderTexture RTT;
    //private Texture texture;

    // Use this for initialization
    void Start () {
        //GetComponent<Material>().mainTexture = GetComponentInChildren<PortalCam>().RTT;
        //RTT = GetComponentInChildren<PortalCam>().RTT;
        //texture = GetComponent<Renderer>().material.mainTexture;

        jumpTimer = Random.Range(60, 90);

        BoxCollider box = GameObject.Find("Environment").GetComponent<BoxCollider>();

        jumpRangeX = box.size.x / 2;
        jumpRangeY = box.size.y / 2;
        jumpRangeZ = box.size.z / 2;

        teleSound = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {

        //GetComponent<Renderer>().material.mainTexture = GetComponentInChildren<PortalCam>().RTT;

        jumpTimer -= Time.deltaTime;
        if (jumpTimer <= 0.0f || Vector3.Distance(transform.position, otherPortal.transform.position) < 2400)
        {
            jumpTimer = Random.Range(60f, 90f);
            transform.position = new Vector3(Random.Range(-jumpRangeX, jumpRangeX), Random.Range(-jumpRangeY, jumpRangeY), Random.Range(-jumpRangeZ, jumpRangeZ));
        }
    }

    void OnBecameVisible()
    {
        enabled = true;
    }

    void OnBecameInvisible()
    {
        enabled = false;
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player")
        {
            col.transform.position = otherPortal.transform.position + otherPortal.transform.forward * 40;
            teleSound.Play();
        }
        else if (col.transform.tag == "Enemy")
        {
            col.transform.position = otherPortal.transform.position + otherPortal.transform.forward * 40;
            col.transform.rotation = new Quaternion(otherPortal.transform.forward.x, otherPortal.transform.forward.y, otherPortal.transform.forward.z, 1);
        }
    }

}
