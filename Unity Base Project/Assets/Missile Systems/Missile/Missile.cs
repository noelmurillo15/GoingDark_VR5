using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour
{

    public float Speed;
    public int LookSpeed;
    public float TimeTillTrack;
    public float Timer;
    public float DistanceTillStopLooking;
    public float CalculatedDistance;
    public Vector3 Target;
    public Quaternion targetRotation;
    public GameObject Enemy;
    public GameObject Transport;
    public GameObject Explosion;
    public bool stopTurning;
    public int TimeTillExpire;
    public bool Die;


    public float DestroyTimer;

    void Start()
    {
        //Find the target object
        //if(gameObject.GetComponentInParent<== "Enemy")
            //FoundTargetObject = GameObject.FindGameObjectWithTag("Target");
        //else if (gameObject.tag == "Player")
        Enemy = GameObject.FindGameObjectWithTag("Enemy");
        Transport = GameObject.FindGameObjectWithTag("TransportShip");

        if(Enemy != null)
            Target = Enemy.transform.position;

        DestroyTimer = 0.0f;
    }

    void Update()
    {

        if (Enemy == null)
            Target = Transport.transform.localPosition;
        else
            Target = Enemy.transform.localPosition;

        if (DestroyTimer > 0.0f)
            DestroyTimer -= Time.deltaTime;


        //set up the timer
        Timer += Time.deltaTime;
        //destroy if missile's time is up
        if (Timer > TimeTillExpire && !Die)
        {
            Die = true;
        }
        //find the distance from missile to target
        CalculatedDistance = Vector3.Distance(gameObject.transform.position, Target);
        //give the missile speed
        if(!Die)
            transform.Translate(0, 0, Speed * Time.deltaTime);
        //delay tracking for a certain amount of time...
        if (Timer > TimeTillTrack && !Die)
        {
            if (stopTurning == false)
            {
                //look at the target object at speed
                targetRotation = Quaternion.LookRotation(Target - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * LookSpeed);
            }
        }
        //set up instances that the missile will die...
       //if (CalculatedDistance < DistanceTillStopLooking)
       //{
       //    stopTurning = true;
       //    Die = true;
       //}
        if (Die == true)
        {
            if (Explosion != null)
            {
                Instantiate(Explosion, transform.position, transform.rotation);
                Explosion = null;
                Destroy(this.transform.GetChild(0).gameObject);
            }

            if(DestroyTimer <= 0.0f)
                Destroy(gameObject, 0);
        }
    }
    void OnTriggerEnter(Collider col)
    {
        Die = true;
        if(col.tag == "Enemy")
        {
            Debug.Log("Missile Hit Enemy Ship");
            DestroyTimer = 2.5f;
            col.gameObject.SendMessage("Kill");
        }
        else if (col.tag == "TransportShip")
        {
            Debug.Log("Missile Hit Transport Ship");
            DestroyTimer = 2.5f;
            col.gameObject.SendMessage("Kill");
        }
    }
}
