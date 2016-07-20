using UnityEngine;

public class PortalEnter : MonoBehaviour {

    public GameObject PortalExit;
    private MissionSystem missionSystem;
    //private AudioSource teleSound;
    
    // Use this for initialization
    void Start () {
        if (PortalExit == null)
            Debug.LogError("Portal Does not have destination attached : Portal Exit == null");
        missionSystem = GameObject.Find("PersistentGameObject").GetComponent<MissionSystem>();
        //teleSound = GetComponent<AudioSource>();
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player")
        {            
            missionSystem.EnteredPortal(gameObject.name);
            col.transform.position = new Vector3(PortalExit.transform.position.x, PortalExit.transform.position.y, PortalExit.transform.position.z + 10f);
            col.transform.rotation = Quaternion.identity;
            //teleSound.Play();
        }
    }
}
