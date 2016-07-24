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
        if (IsInvoking("Kill"))
            CancelInvoke("Kill");

        gameObject.SetActive(false);
    }
}
