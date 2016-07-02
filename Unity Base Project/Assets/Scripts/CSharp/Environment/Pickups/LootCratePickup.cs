using UnityEngine;

public class LootCratePickup : MonoBehaviour{

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player")
        {

            int Credits = PlayerPrefs.GetInt("Credits",0);
          //  Debug.Log(Credits + "Credits at the start");
            Credits += 100;
            PlayerPrefs.SetInt("Credits", Credits);
           // Debug.Log(Credits + "Credits at the end");

            Destroy(this.gameObject);
            AudioManager.instance.PlayCollect();
        }
    }
}
