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
    public bool startTracking;


    public float DestroyTimer;

    void Start() {
        Enemy = GameObject.FindGameObjectWithTag("Enemy");
        Transport = GameObject.FindGameObjectWithTag("TransportShip");

        if(Enemy != null)
            Target = Enemy.transform.position;

        Die = false;

        Speed = 50.0f;
        startTracking = false;
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

        //Timer += Time.deltaTime;
        //if (Timer > TimeTillExpire && !Die)
        //    Die = true;

        CalculatedDistance = Vector3.Distance(gameObject.transform.position, Target);

        if(!Die)
            transform.Translate(0, 0, Speed * Time.deltaTime);

        if (!Die && startTracking)
        {
            targetRotation = Quaternion.LookRotation(Target - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * LookSpeed);            
        }

        if (Die)
        {
            if (Explosion != null)
            {
                Instantiate(Explosion, transform.position, transform.rotation);
                Explosion = null;
                this.GetComponent<MeshRenderer>().enabled = false;
            }

            if(DestroyTimer <= 0.0f)
                Destroy(this.gameObject, 0);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "TransportShip")
        {
            startTracking = true;
            Debug.Log("Begin Missile Tracking");
        }
    }
    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Collision with : " + col.gameObject.name);
        if(col.gameObject.tag == "Enemy")
        {
            Die = true;
            Debug.Log("Missile Hit Enemy Ship");
            DestroyTimer = 2.5f;
            col.gameObject.SendMessage("Kill");
        }
        else if (col.gameObject.tag == "TransportShip")
        {
            Debug.Log("Missile Hit Transport Ship");
            DestroyTimer = 5.0f;
            col.gameObject.SendMessage("Kill");
        }
    }
}
