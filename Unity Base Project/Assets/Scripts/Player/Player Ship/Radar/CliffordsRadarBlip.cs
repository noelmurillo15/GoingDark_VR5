using UnityEngine;
using System.Collections;

// <Setting up the blip>
// obj = instantiate(blip)
// obj.transform.parent = transform;
// obj.sendmessage("SetTimer", time)
// obj.sendmessage("SetEnemy", time)
// </Setting up the blip>



public class CliffordsRadarBlip : MonoBehaviour
{
    [SerializeField]
    private Vector3 Direction;
    [SerializeField]
    private GameObject EnemyHandle;
    //Will need to change on RadarPlane as well
    
    private Vector3 ScalerFactor;// = new Vector3(0.003f, 0.003f, 0.003f);
    [SerializeField]
    private Color LineColor;
    [SerializeField]
    private Material MaterialColorRed;
    [SerializeField]
    private Material MaterialColorBlue;
    [SerializeField]
    private Material MaterialColorYellow;
    [SerializeField]
    private Material MaterialColorOther;

    GameObject Player;

    // Use this for initialization
    void Start()
    {

        Player = GameObject.FindGameObjectWithTag("Player");
        // transform.localPosition.Set(0.0f, 0.0f, 0.0f);
        //transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        //Destroy(gameObject, 60);
        ScalerFactor.x = .25f / GetComponentInParent<SphereCollider>().radius;
        ScalerFactor.y = .25f  / GetComponentInParent<SphereCollider>().radius;
        ScalerFactor.z = .25f / GetComponentInParent<SphereCollider>().radius;
    }

    // Update is called once per frame
    void Update()
    {

        // Debug.Break();
        // Debug.Log(" Updated in blip");
        if (EnemyHandle == null)
        {// if no Enemy or it has been destoryed kill the Blip
            Destroy(gameObject);
            return;
        }

        //keep updating the position
        //Debug.Log(EnemyHandle.GetComponent<Transform>().localPosition + " Enemy Position");
        //Debug.Log(Player.GetComponent<Transform>().localPosition + " Player Position");
        Vector3 tempVec = EnemyHandle.transform.position - Player.transform.position;
        //Debug.Log(tempVec + " Temp Position");

        //taking away the radar plane position;
        //Scale
        tempVec.Scale(ScalerFactor);
        //Debug.Log(tempVec + " * Scale");

    //    Debug.Log(transform.localPosition);
        //GetComponent<Transform>().localPosition.Set(tempVec.x, tempVec.y, tempVec.z);
        transform.localPosition = tempVec;
      //  Debug.Log(transform.localPosition +" Setting = to TempVec: " + tempVec);


        //ping effect for polish;



        //drawline(toplane)
        Vector3 TempPositionForY = transform.localPosition;
        //Get this object's Line Render
        LineRenderer LineToDraw = GetComponent<LineRenderer>();
        //Set first position from Blip
        LineToDraw.SetPosition(0, TempPositionForY);
        //Change in Y for the plane
        TempPositionForY.y = transform.parent.transform.localPosition.y;
        //Set Position to Draw straight up/down to the plane.
        LineToDraw.SetPosition(1, TempPositionForY);
        //Rotate(towards(direction));
        Vector3 newRot = EnemyHandle.transform.localEulerAngles;
        //newRot.y -= 180.0f;
        newRot.x -= 180.0f;
        transform.localEulerAngles = newRot;
        LineToDraw.SetColors(LineColor, LineColor);
        LineToDraw.SetWidth(.001f, .001f);

    }

    void SetEnemy(GameObject enemy)//Getting the Enemy handle to repersent on Plane
    {
        // Debug.Log(EnemyHandle.tag);
        EnemyHandle = enemy;
        if (enemy.tag == "Enemy")
        {
            LineColor = Color.red;
            GetComponent<Renderer>().material = MaterialColorRed;
        }
        else if (enemy.tag == "Loot")
        {
            LineColor = Color.blue;
            GetComponent<Renderer>().material = MaterialColorBlue;

        }
        else if (enemy.tag == "TransportShip")
        {
            LineColor = Color.yellow;
            GetComponent<Renderer>().material = MaterialColorYellow;
        }
    }
    //void SetTimer(float TimeLeftOnSonar) // Time left until destoryed because of Sonar Shutting off.
    //{
    //    Destroy(gameObject, TimeLeftOnSonar);
    //}
}

