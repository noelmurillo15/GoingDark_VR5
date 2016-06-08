using UnityEngine;
using System.Collections;

public class TestGamepad : MonoBehaviour {

    x360Controller controller;
	// Use this for initialization
	void Start () {
        controller = GamePadManager.Instance.GetController(0);
	}
	
	// Update is called once per frame
	void Update () {

        if (controller == null)
        {

            Debug.Log("controller not connected");
            return;
        }

        if (controller.GetButtonDown("A"))
        {
            Rumble();
            Debug.Log("Pressed A");
        }
	}


    void Rumble()
    {
        controller.AddRumble(1.0f, new Vector2(1, 1), 2.0f);
        controller.AddRumble(2.0f, new Vector2(1,1), 3.0f);
    }
}
