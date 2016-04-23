using UnityEngine;
using System.Collections;

public class CliffordsRadarPlane : MonoBehaviour
{
    //Will need to change on RadarBlip as well
    [SerializeField]
    private Vector3 ScalerFactor = new Vector3(0.003f, 0.003f, 0.003f);
    [SerializeField]
    private GameObject PreFabBlip;
    [SerializeField]
    private float SonarTimeLeft = 10;


    private int Counter = 0;
    private GameObject[] TheObject;
    private GameObject[] TheBlip;
    

    // Use this for initialization
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {


    }

    void OnTriggerEnter(Collider ColliderObject)//hey dummy what hit ya?
    {
        //Debug.Log("Collision Detected with "+ ColliderObject.gameObject.tag);
        
        if (ColliderObject.gameObject.tag == "Enemy" || ColliderObject.gameObject.tag == "Loot" || ColliderObject.gameObject.tag == "TransportShip")
       {



            Vector3 ColliderPosition = ColliderObject.transform.localPosition; // There he is! Get Him! 
           // Debug.Log(ColliderPosition + " Poisiton of Enemy");
           // Debug.Log(GetComponentInParent<Transform>().localPosition + " Poistion of Player");

            Vector3 PositionOfEnemy = ColliderPosition - GetComponentInParent<Transform>().localPosition;// world space of enemy - world space of player = building a vector from player to enemy; 
            //Debug.Log(PositionOfEnemy + " Position of Enemy - Player");
            // vector mag how far in that direction.

            //Scale it by Scaler factor to get smaller vector position(;
            PositionOfEnemy.Scale(ScalerFactor);
            //Debug.Log(PositionOfEnemy + " * by Scaler");

         //   Quaternion ColliderRotation = ColliderObject.transform.rotation; //Gobal Space Rotation!(;


          //  GameObject Blip = (GameObject)Instantiate(PreFabBlip); //, PositionOfEnemy, Quaternion.identity); // Making the new object giving it a position and rotation.
            GameObject Blip = Instantiate(PreFabBlip, PositionOfEnemy, Quaternion.identity) as GameObject;

            Blip.transform.SetParent(transform);
            Blip.transform.localPosition.Set(0.0f, 0.0f, 0.0f);
            //((GameObject)Blip).transform.parent = this.transform; // Setting the Blip objects parent to this Plane's Transform
            Blip.GetComponent<CliffordsRadarBlip>().SendMessage("SetTimer", SonarTimeLeft); // Setting a destory!... timer..
            Blip.GetComponent<CliffordsRadarBlip>().SendMessage("SetEnemy", ColliderObject.gameObject);// giving the Blip object the GameObject for further destruction(Updating).


            Counter++;// 0 -> 1
            System.Array.Resize(ref TheObject, Counter); // make space
            TheObject[Counter - 1] = ColliderObject.gameObject; // put it in at 0 to start;

            System.Array.Resize(ref TheBlip, Counter); // make space
            TheBlip[Counter - 1] = Blip; // put it in at 0 to start;

        }

    }
    void OnTriggerExit(Collider ColliderObject)//hey he's leaving!
    {
        for (int i = 0; i < Counter; i++)
            if (ColliderObject.gameObject == TheObject[i]) // see if they match
                Destroy(TheBlip[i]); // destory the blip gameobject
    }

}