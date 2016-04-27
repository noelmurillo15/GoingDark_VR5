using UnityEngine;
using System.Collections;

public class ReOrientPlayer : MonoBehaviour {

    private GameObject Player;

    // Use this for initialization
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        if(col.name == "bone3")
        {
            Debug.Log("Re-orienting Player Rotation");
            Player.SendMessage("Reorient");
        }
    }
}
