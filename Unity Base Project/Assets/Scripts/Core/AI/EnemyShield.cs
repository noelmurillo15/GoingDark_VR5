using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    public bool alive = true;
    private float health = 100.0f;
    private GameObject shield;
    private GameObject shield2;
    private GameObject miss;


    // Use this for initialization
    void Start()
    {
        shield = transform.GetChild(3).gameObject;
        shield2 = transform.GetChild(4).gameObject;
        miss = GameObject.Find("Missile");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage()
    {
        AudioManager.instance.PlayShieldHit();
        health -= 50.0f;
        if(health == 50.0f)
        {
            //alive = false;
            shield.SetActive(false);
            
        }
        if (health == 0.0f)
        {
            alive = false;
            shield2.SetActive(false);
            Debug.Log("Shields Down!!!!");
        }
        else if(health <= -1.0f)
        {
            alive = false;
        }
    }
    
}
