using UnityEngine;

public class Radar2DScript : MonoBehaviour
{

    GameObject Player;
    GameObject[] RadarImages;
    private int[] QuadCounter;
    private int TotalObjectCount = 0;
    private GameObject[] TheObject;
    private int[] Objectsquad;



    // Use this for initialization
    void Start()
    {
        Player = GameObject.Find("Player");
        RadarImages = new GameObject[9];
        QuadCounter = new int[9];
        TheObject = new GameObject[2];
        Objectsquad = new int[1];
        for (int i = 0; i < 9; i++)
        {
            //            this.transform.GetChild(i).gameObject.SetActive(false);
            RadarImages[i] = this.transform.GetChild(i).gameObject;
            RadarImages[i].SetActive(false);
        }
    }

    void OnTriggerEnter(Collider ColliderObject)//hey dummy what hit ya?
    {
        if (ColliderObject.CompareTag("Enemy") && ColliderObject.gameObject.activeSelf == true)
        {
            TotalObjectCount++;//how many objects to keep track of.
            System.Array.Resize(ref TheObject, TotalObjectCount); // make space for the new object
            TheObject[(TotalObjectCount - 1)] = ColliderObject.gameObject; // put it in at 0 to start;


            System.Array.Resize(ref Objectsquad, TotalObjectCount); // make space for the new object (Quad numbering)


            Vector3 ColliderPosition = ColliderObject.transform.position; // Object's position either loot missile or enemy; 
            Vector3 TargetDir = ColliderPosition - Player.transform.position;//target direction

            TargetDir.Normalize();
            float angle = Vector3.Dot(TargetDir, Player.transform.forward); //angle between my heading and and the target direction;

            float HalfSpaceTest = Vector3.Dot(TargetDir, Player.transform.right);

            angle += 1;
            angle *= 90;
            angle = 180 - angle;
            if (HalfSpaceTest < 0) // on the left
            {
                angle *= -1;
            }

            //distance check.
            //if(distance between enemy and me is less than EMP range)
            //Light up quad 8;
            //If closer than emp burst radius = 8 quad

            TurnOnRadarPanels(angle);
        }
    }

    void OnTriggerExit(Collider ColliderObject)
    {
        for (int i = 0; i < TheObject.Length; i++)
            if (ColliderObject.gameObject == TheObject[i]) // see if they match
            {
                TotalObjectCount--;
            }

    }


    //0   - 259 degrees
    void TurnOnRadarPanels(float angle)
    {
        int Quad = -1;

        if (angle <= 45.0f && angle > 0) //Top half of radar
            Quad = 0;
        else if (angle <= 90 && angle > 0)
            Quad = 1;
        else if (angle <= 135 && angle > 0)
            Quad = 2;
        else if (angle <= 180 && angle > 0)
            Quad = 3;
        else if (angle >= -45.0f && angle <= 0) //Top half of radar
            Quad = 7;
        else if (angle >= -90)
            Quad = 6;
        else if (angle >= -135)
            Quad = 5;
        else if (angle >= -180)
            Quad = 4;


        if (Quad != -1)
        {
            QuadCounter[Quad]++;
            if (QuadCounter[Quad] > 0)
                RadarImages[Quad].gameObject.SetActive(true);
            Objectsquad[(TotalObjectCount - 1)] = Quad; //Tells me what quad they are in.
        }

    }
    void TurnOffRadarPanels()
    {
        for (int i = 0; i < 9; i++)
            RadarImages[i].SetActive(false);
    }

    void Update()
    {
        TurnOffRadarPanels();
        for (int i = 0; i < TheObject.Length; i++)
        {
            if (TheObject[i] != null && TheObject[i].activeInHierarchy)
            {
                Vector3 ColliderPosition = TheObject[i].transform.position; // Object's position either loot missile or enemy; 
                Vector3 TargetDir = ColliderPosition - Player.transform.position;//target direction

                TargetDir.Normalize();
                float angle = Vector3.Dot(TargetDir, Player.transform.forward); //angle between my heading and and the target direction;

                float HalfSpaceTest = Vector3.Dot(TargetDir, Player.transform.right);

                angle += 1;
                angle *= 90;
                angle = 180 - angle;
                if (HalfSpaceTest < 0)
                    angle *= -1;

                //distance check.
                float dist = Vector3.Distance(ColliderPosition, Player.transform.position);

                if (dist < 500)
                    RadarImages[8].SetActive(true);


                TurnOnRadarPanels(angle);
            }
            //ObjectLeftRadar(Objectsquad[i]);
        }

    }

    void ObjectLeftRadar(int quad)
    {
        if (QuadCounter[quad] > 1)
            QuadCounter[quad]--;
        else
        {
            QuadCounter[quad]--;
            RadarImages[quad].gameObject.SetActive(false);
        }
    }

}
