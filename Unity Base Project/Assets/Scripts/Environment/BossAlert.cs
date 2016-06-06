using UnityEngine;
using System.Collections;

public class BossAlert : MonoBehaviour
{
    private GameObject message;

    // Use this for initialization
    void Start()
    {
        message = GameObject.Find("WarningMessages");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player")
        {
           StartCoroutine(message.GetComponent<MessageScript>().BossAlert());
        }
    }
}
