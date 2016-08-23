using UnityEngine;
using System.Collections;

public class Radar2DScript : MonoBehaviour
{

    GameObject Player;
    GameObject[] RadarImages;
    GameObject EnemyClose;
    ArrayList EnemiesArray = new ArrayList();
    private int EMP_Distance = 500;
    int EnemyCount;


    // Use this for initialization
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");// find player tutorial eventually
        RadarImages = new GameObject[9];
        EnemyClose = GameObject.Find("EnemyClose");
        EnemyCount = 0;

        for (int i = 0; i < 9; i++)
        {
            //            this.transform.GetChild(i).gameObject.SetActive(false);
            RadarImages[i] = transform.GetChild(i).gameObject;
            RadarImages[i].SetActive(false);
        }
    }

    void OnTriggerEnter(Collider ColliderObject)//hey dummy what hit ya?
    {
        if (ColliderObject.CompareTag("Enemy") && ColliderObject.gameObject.activeSelf == true)
        {
            //Add enemy to arraylist
            if (!EnemiesArray.Contains(ColliderObject.gameObject))
            {
                EnemiesArray.Add(ColliderObject.gameObject);
                EnemyCount++;
            }//End of adding

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
        {
            EnemiesArray.Remove(ColliderObject.gameObject);
            EnemyCount--;
        }
    }


    //0   - 259 degrees
    void TurnOnRadarPanels(float angle)
    {
        //know what quad to light up.
        int Quad = -1;

        #region Where are they in the circle?
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

        // Turn on the quad if it has changed.
        if (Quad != -1)
            RadarImages[Quad].gameObject.SetActive(true);
        #endregion

        #region Middle of Lines

        if (angle <= 15 && angle > -15) //ahead top left and right turned on.
            if (Quad == 0)
                Quad = 7;
            else
                Quad = 0;
        else if (angle <= 65 && angle > 35) // top right and right
            if (Quad == 0)
                Quad = 1;
            else
                Quad = 0;
        else if (angle <= 105 && angle > 75) // right and bottom top right
            if (Quad == 1)
                Quad = 2;
            else
                Quad = 1;
        else if (angle <= 165 && angle > 150) //bottom top right and bottom right
            if (Quad == 2)
                Quad = 3;
            else
                Quad = 2;
        else if (angle >= 165 && angle < -165) //bottom right and bottom left
            if (Quad == 3)
                Quad = 4;
            else
                Quad = 3;
        else if (angle >= -120 && angle < -150) //bottom left and bottom top left
            if (Quad == 4)
                Quad = 5;
            else
                Quad = 4;
        else if (angle >= -75 && angle < -105) //bottom top left and left
            if (Quad == 5)
                Quad = 6;
            else
                Quad = 5;
        else if (angle >= -30 && angle < -60) //bottom right and bottom left
            if (Quad == 6)
                Quad = 7;
            else
                Quad = 6;

        if (Quad != -1)
            RadarImages[Quad].gameObject.SetActive(true);
        #endregion
    }
    void TurnOffRadarPanels()
    {
        for (int i = 0; i < 9; i++)
            RadarImages[i].SetActive(false);
    }

    void LateUpdate()
    {
        TurnOffRadarPanels(); // turn them all off.
        for (int i = 0; i < EnemiesArray.Count; i++) // loop threw the array and set enemies quads.
        {
            GameObject temp = (GameObject)(EnemiesArray[i]);//get the game object
            if (temp != null && temp.activeInHierarchy) // check to make sure it is active in the scene
            {
                Vector3 ColliderPosition = temp.transform.position; // Object's position either loot missile or enemy; 
                Vector3 TargetDir = ColliderPosition - Player.transform.position;//target direction

                TargetDir.Normalize();
                float angle = Vector3.Dot(TargetDir, Player.transform.forward); //angle between my heading and and the target direction;

                float HalfSpaceTest = Vector3.Dot(TargetDir, Player.transform.right); // halfspace test

                angle += 1;
                angle *= 90;
                angle = 180 - angle;

                if (HalfSpaceTest < 0)
                    angle *= -1;

                //distance check.
                float dist = Vector3.Distance(ColliderPosition, Player.transform.position);

                if (dist < EMP_Distance) // checked against EMP distance for middle circle to light up.
                    RadarImages[8].SetActive(true);

                EnemyClose.GetComponent<EnemyCloseScript>().SetEnemyCloseText(EnemyCount);
                TurnOnRadarPanels(angle);//turn on that quad.
            }
            else if (EnemiesArray.Contains(temp) || temp == null)
            {
                EnemiesArray.Remove(temp);
                EnemyCount--;
            }

        }
    }

    public void ClearAll()
    {
        EnemiesArray.Clear();
        EnemyCount = 0;
    }
}