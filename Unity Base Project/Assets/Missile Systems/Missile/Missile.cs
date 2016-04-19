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

    void Start()
    {
        //Find the target object
        //if(gameObject.GetComponentInParent<== "Enemy")
            //FoundTargetObject = GameObject.FindGameObjectWithTag("Target");
        //else if (gameObject.tag == "Player")
        Enemy = GameObject.FindGameObjectWithTag("Enemy");
        Transport = GameObject.FindGameObjectWithTag("TransportShip");
        Target = Enemy.transform.position;
    }

    void Update()
    {

        if (Enemy == null)
            Target = Transport.transform.localPosition;
        else
            Target = Enemy.transform.localPosition;


        //set up the timer
        Timer += Time.deltaTime;
        //destroy if missile's time is up
        if (Timer > TimeTillExpire)
        {
            Die = true;
        }
        //find the distance from missile to target
        CalculatedDistance = Vector3.Distance(gameObject.transform.position, Target);
        //give the missile speed
        transform.Translate(0, 0, Speed / 100);
        //delay tracking for a certain amount of time...
        if (Timer > TimeTillTrack)
        {
            if (stopTurning == false)
            {
                //look at the target object at speed
                targetRotation = Quaternion.LookRotation(Target - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * LookSpeed);
            }
        }
        //set up instances that the missile will die...
        if (CalculatedDistance < DistanceTillStopLooking)
        {
            stopTurning = true;
            Die = true;
        }
        if (Die == true)
        {
            Instantiate(Explosion, transform.position, transform.rotation);
            Destroy(gameObject, 0);
        }
    }
    void OnCollisionEnter(Collision col)
    {
        Die = true;
        if(col.gameObject.tag == "Enemy")
        {
            col.gameObject.SendMessage("Kill");
            Enemy = null;
        }
        else if (col.gameObject.tag == "TransportShip")
        {
            col.gameObject.SendMessage("Kill");
            Transport = null;
        }
    }
}
