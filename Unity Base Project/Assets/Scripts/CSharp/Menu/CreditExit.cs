using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditExit : MonoBehaviour
{

    x360Controller m_Controller;
    float width, height;
    // Use this for initialization
    void Start()
    {
        m_Controller = GamePadManager.Instance.GetController(0);

        ResetPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Controller.GetButtonDown("B"))
        {
            SceneManager.LoadScene("MainMenu");
        }
        if (width != Screen.width || Screen.height != height)
        {
            ResetPos();
        }
    }

    void ResetPos()
    {
        width = Screen.width;
        height = Screen.height;
        Vector3 temp = new Vector3(width - 100, 0 + 50, 0);
        transform.position = temp;
    }
}
