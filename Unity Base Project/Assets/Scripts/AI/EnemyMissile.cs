using UnityEngine;
using System.Collections;

public class EnemyMissile : MonoBehaviour {

    public float Speed;
    public int LookSpeed;
    public float TimeTillTrack;
    public float Timer;
    public float DistanceTillStopLooking;
    public float CalculatedDistance;
    public Quaternion targetRotation;
    public GameObject Player;
    public GameObject Explosion;
    public bool stopTurning;
    public int TimeTillExpire;
    public bool Die;
    public bool startTracking;

    private Vector3 Target;

    public float DestroyTimer;

    //void Awake()
    //{
    //    startTracking = true;
    //}

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        if (Player != null)
            Target = Player.transform.position;

        Die = false;

        Speed = 50.0f;
        DestroyTimer = 10.0f;
        LookSpeed = 2;
    }

    void Update()
    {

        if (Player != null)
            Target = Player.transform.localPosition;

        if (DestroyTimer > 0.0f)
            DestroyTimer -= Time.deltaTime;

        CalculatedDistance = Vector3.Distance(gameObject.transform.position, Target);

        if (!Die)
            transform.Translate(0, 0, Speed * Time.deltaTime);

        if (!Die && startTracking)
        {
            targetRotation = Quaternion.LookRotation(Target - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * LookSpeed);
        }

        if (DestroyTimer <= 0.0f)
            Destroy(this.gameObject, 0);

        if (Die)
        {
            if (Explosion != null)
            {
                Instantiate(Explosion, transform.position, transform.rotation);
                Explosion = null;
                this.GetComponent<MeshRenderer>().enabled = false;
            }

            if (DestroyTimer <= 0.0f)
                Destroy(this.gameObject, 0);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Collision with : " + col.gameObject.name);
       
        if (col.gameObject.tag == "Player")
        {
           Die = true;
           Debug.Log("Missile Hit Player");
           DestroyTimer = 2.5f;
           col.gameObject.SendMessage("Hit");
        }
    }
}
