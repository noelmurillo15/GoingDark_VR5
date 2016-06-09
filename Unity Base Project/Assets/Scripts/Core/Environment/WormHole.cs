using UnityEngine;
using System.Collections;

public class WormHole : MonoBehaviour {

    Transform myTransform;
    private float jumpRangeX, jumpRangeY, jumpRangeZ, offsetZ;
    private AudioSource teleSound;
    private float jumpTimer;
    
    // Use this for initialization
    void Start () {
        myTransform = transform;
        jumpTimer = Random.Range(120, 180);

        BoxCollider box = GameObject.Find("OpenWorldStreamer").GetComponent<BoxCollider>();
        
        jumpRangeX = box.size.x / 2;
        jumpRangeY = box.size.y / 2;
        jumpRangeZ = box.size.z / 2;
        offsetZ = box.center.z;
        
        teleSound = GetComponent<AudioSource>();
        myTransform.position = new Vector3(Random.Range(-jumpRangeX, jumpRangeX), Random.Range(-jumpRangeY, jumpRangeY), Random.Range(-jumpRangeZ + offsetZ, jumpRangeZ + offsetZ));
    }

    // Update is called once per frame
    void Update () {
        myTransform.Rotate(Vector3.up, 2 * Time.deltaTime);

        jumpTimer -= Time.deltaTime;
        if (jumpTimer <= 0.0f)
        {
            jumpTimer = Random.Range(120f, 180f);
            myTransform.position = new Vector3(Random.Range(-jumpRangeX, jumpRangeX), Random.Range(-jumpRangeY, jumpRangeY), Random.Range(-jumpRangeZ + offsetZ, jumpRangeZ + offsetZ));
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
            Vector3 randPos = new Vector3(Random.Range(-jumpRangeX, jumpRangeX), Random.Range(-jumpRangeY, jumpRangeY), Random.Range(-jumpRangeZ + offsetZ, jumpRangeZ + offsetZ));
            while (Vector3.Distance(myTransform.position, randPos) < 1500)
                randPos = new Vector3(Random.Range(-jumpRangeX, jumpRangeX), Random.Range(-jumpRangeY, jumpRangeY), Random.Range(-jumpRangeZ + offsetZ, jumpRangeZ + offsetZ));
            
            col.transform.position = randPos;
            col.transform.rotation = Quaternion.identity;
            teleSound.Play();
        }
        else if (col.transform.tag == "Enemy")
        {
            Vector3 randPos = new Vector3(Random.Range(-jumpRangeX, jumpRangeX), Random.Range(-jumpRangeY, jumpRangeY), Random.Range(-jumpRangeZ + offsetZ, jumpRangeZ + offsetZ));

            while (Vector3.Distance(myTransform.position, randPos) < 1500)
                randPos = new Vector3(Random.Range(-jumpRangeX, jumpRangeX), Random.Range(-jumpRangeY, jumpRangeY), Random.Range(-jumpRangeZ + offsetZ, jumpRangeZ + offsetZ));

            Vector3 newpos = col.transform.position += randPos;
            col.transform.position = newpos;
            col.transform.rotation = Quaternion.identity;
            teleSound.Play();
        }
    }
}
