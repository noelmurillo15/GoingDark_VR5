using UnityEngine;
using System.Collections;

public class Radar2DScript : MonoBehaviour
{

    GameObject Player;
    GameObject[] RadarImages;
    private int[] QuadCounter;
    private int TotalObjectCount = 0;
    ArrayList EnemiesArray = new ArrayList();


    // Use this for initialization
    void Start()
    {
        Player = GameObject.Find("PlayerTutorial");// find player tutorial eventually
        RadarImages = new GameObject[9];
        QuadCounter = new int[9];


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
            //Add enemy to arraylist
            EnemiesArray.Add(ColliderObject.gameObject);
            //End of adding

            //Do math figure out where it is.
            Vector3 ColliderPosition = ColliderObject.transform.position; // Object's position either loot missile or enemy; 
            Vector3 TargetDir = ColliderPosition - Player.transform.position;//target direction
            TargetDir.Normalize();
            float angle = Vector3.Dot(TargetDir, Player.transform.forward); //angle between my heading and and the target direction;
            float HalfSpaceTest = Vector3.Dot(TargetDir, Player.transform.right);

            //fix the angle
            angle += 1;
            angle *= 90;
            angle = 180 - angle;
            if (HalfSpaceTest < 0) // on the left
            {
                angle *= -1;
            }

            TurnOnRadarPanels(angle);
        }
    }

    void OnTriggerExit(Collider ColliderObject)
    {
        if (EnemiesArray.Contains(ColliderObject.gameObject))
            EnemiesArray.Remove(ColliderObject.gameObject);
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
        for (int i = 0; i < EnemiesArray.Count; i++)
        {
            GameObject temp = (GameObject)(EnemiesArray[i]);
            if (temp != null && temp.activeInHierarchy)
            {
                Vector3 ColliderPosition = temp.transform.position; // Object's position either loot missile or enemy; 
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
        }

    }
}