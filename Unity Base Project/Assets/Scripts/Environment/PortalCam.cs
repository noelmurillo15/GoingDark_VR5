using UnityEngine;
using System.Collections;

public class PortalCam : MonoBehaviour {
   
    //public RenderTexture RTT;
	// Use this for initialization
	void Start () {
        

        //RTT = RenderTexture.active;
        //RTT = new RenderTexture(2048, 2048, 24, RenderTextureFormat.ARGB32);
        //RTT.filterMode = FilterMode.Bilinear;
        //RTT.wrapMode = TextureWrapMode.Clamp;
        //GetComponent<Camera>().targetTexture = RTT;
        
    }
	
	// Update is called once per frame
	void Update () {
        
        transform.Rotate(Vector3.up, 2 * Time.deltaTime);
	}
}
