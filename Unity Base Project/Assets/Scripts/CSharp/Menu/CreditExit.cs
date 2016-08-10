using UnityEngine;
using System.Collections;

public class CreditExit : MonoBehaviour {

    x360Controller m_Controller;
    float width, height;
    // Use this for initialization
    void Start () {
        m_Controller = GamePadManager.Instance.GetController(0);

        width = Screen.width;
        height = Screen.height;

        Vector3 temp = new Vector3(width - 100, 0 + 50, 0);
        transform.position = temp;
    }

    // Update is called once per frame
    void Update () {
        if (m_Controller.GetButtonDown("B"))
        {
            LoadingScreenManager.LoadScene("MainMenu");
        }
        if (width != Screen.width || Screen.height != height)
        {
            width = Screen.width;
            height = Screen.height;
            Vector3 temp = new Vector3(width - 100, 0 + 50, 0);
            transform.position = temp;
        }
    }
}
