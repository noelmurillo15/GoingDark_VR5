using UnityEngine;
using System.Collections;

public class EnemyMissile : MonoBehaviour {
    //**        Attach to Enemy Missile Prefab      **//

    public bool Die;
    public bool startTracking;

    public float Speed;
    public float DestroyTimer;

    public int LookSpeed;

    private Vector3 Target;
    public Quaternion targetRotation;

    public GameObject Player;
    public GameObject Explosion;


    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        Player.GetComponent<ThirdPersonVisor>().SetIncomingMissileWarning(true);

        if (Player != null)
            Target = Player.transform.position;

        Die = false;

        Speed = 80.0f;
        DestroyTimer = 6.0f;
        LookSpeed = 5;
    }

    void Update()  {

        if (Player != null)
            Target = Player.transform.localPosition;

        if (DestroyTimer > 0.0f)
            DestroyTimer -= Time.deltaTime;
        else {
            Player.GetComponent<ThirdPersonVisor>().SetIncomingMissileWarning(false);
            Destroy(this.gameObject, 0);
        }

        if (!Die)
            transform.Translate(0, 0, Speed * Time.deltaTime);

        if (!Die && startTracking)
        {
            targetRotation = Quaternion.LookRotation(Target - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * LookSpeed);
        }

        if (Die && Explosion != null)
        {
            Instantiate(Explosion, transform.position, transform.rotation);
            Explosion = null;
            this.GetComponent<MeshRenderer>().enabled = false;
        }        
    }

    void OnCollisionEnter(Collision col) {       
        if (col.gameObject.tag == "Player") {
           Debug.Log("Missile Hit Player");
           Die = true;
           col.gameObject.SendMessage("Hit");
        }
    }
}
