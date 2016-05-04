using UnityEngine;

public class EnemyManuevering : MonoBehaviour {
    //**    Attach to a Gameobject you want to move & avoid Asteroids   **//
    public int range = 50;
    public float speed = 25.0f;
    public float rotationSpeed = 20.0f;
    public bool isThereAnyThing = false;

    // Specify the target for the enemy.
    public GameObject target;
    private RaycastHit hit;


    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Look At Target if no obstacles in your path
        if (!isThereAnyThing)
        {
            Vector3 relativePos = target.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
        }

        // Move forward in the world
        transform.Translate(Vector3.forward * Time.deltaTime * speed);


        CheckRayCasts();
        

        // Now Two More RayCast At The End of Object to detect that object has already pass the obsatacle.
        // Just making this boolean variable false it means there is nothing in front of object.
        if (Physics.Raycast(transform.position - (transform.forward * 4), transform.right, out hit, (range / 2.0f)) ||
        Physics.Raycast(transform.position - (transform.forward * 4), -transform.right, out hit, (range / 2.0f)))
        {
            if (hit.collider.gameObject.CompareTag("Asteroid"))
            {
                isThereAnyThing = false;
            }
        }

        // Use to debug the Physics.RayCast.
        Debug.DrawRay(transform.position + (transform.right * 7), transform.forward * range, Color.red);

        Debug.DrawRay(transform.position - (transform.right * 7), transform.forward * range, Color.red);

        Debug.DrawRay(transform.position - (transform.forward * 4), -transform.right * (range / 2.0f), Color.yellow);

        Debug.DrawRay(transform.position - (transform.forward * 4), transform.right * (range / 2.0f), Color.yellow);
    }

    void CheckRayCasts() {
        Transform leftRay = transform;
        Transform rightRay = transform;

        if (Physics.Raycast(rightRay.position + (transform.right * 7), transform.forward, out hit, range))
        {
            if (hit.collider.gameObject.CompareTag("Asteroid"))
            {
                Debug.Log("Right Raycast Hit");
                isThereAnyThing = true;
                transform.Rotate(Vector3.down * Time.deltaTime * rotationSpeed);
                return;
            }
        }
        if (Physics.Raycast(leftRay.position - (transform.right * 7), transform.forward, out hit, range))
        {
            if (hit.collider.gameObject.CompareTag("Asteroid"))
            {
                Debug.Log("Left Raycast Hit");
                isThereAnyThing = true;
                transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
                return;
            }
        }
    }
}