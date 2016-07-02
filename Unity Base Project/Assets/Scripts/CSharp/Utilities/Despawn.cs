using UnityEngine;

public class Despawn : MonoBehaviour
{
    bool init = false;
    public float Duration = 3f;

    void OnEnable()
    {
        if (!init)
        {
            init = true;
            gameObject.SetActive(false);
        }
        else
        {
            Invoke("Kill", Duration);
        }
    }

    // Update is called once per frame
    public void Kill()
    {
        CancelInvoke();
        gameObject.SetActive(false);
    }
}
