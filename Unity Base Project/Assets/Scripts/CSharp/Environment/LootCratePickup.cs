using UnityEngine;

public class LootCratePickup : MonoBehaviour
{

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player")
        {
            AudioManager.instance.PlayCollect();
            col.transform.SendMessage("UpdateCredits", PlayerPrefs.GetInt("Credits") + 100);
            Destroy(gameObject);
        }
    }
}
