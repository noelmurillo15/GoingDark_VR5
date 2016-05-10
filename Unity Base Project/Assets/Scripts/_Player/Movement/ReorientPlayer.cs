using UnityEngine;

public class ReorientPlayer : MonoBehaviour {
    private GameObject Player;

    // Use this for initialization
    void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 rot = Player.transform.localEulerAngles;
        rot.x -= 180;
        transform.localEulerAngles = rot;

    }
    void OnTriggerEnter(Collider col)
    {
        if (col.name == "bone3")
            Player.GetComponent<PlayerMovement>().SendMessage("Reorient");
    }

}
