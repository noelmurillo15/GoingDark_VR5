using UnityEngine;
using System.Collections;

public class WormHole : MonoBehaviour {

    Transform myTransform;
    private float jumpRangeX, jumpRangeY, jumpRangeZ, offsetZ;
    private AudioSource teleSound;
    
    // Use this for initialization
    void Start () {
        myTransform = transform;        
        teleSound = GetComponent<AudioSource>();
        myTransform.position = new Vector3(Random.Range(-jumpRangeX, jumpRangeX), Random.Range(-jumpRangeY, jumpRangeY), Random.Range(-jumpRangeZ + offsetZ, jumpRangeZ + offsetZ));
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
    }
}
