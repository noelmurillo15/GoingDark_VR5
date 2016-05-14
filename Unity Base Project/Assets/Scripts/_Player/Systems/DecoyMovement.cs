using UnityEngine;

public class DecoyMovement : MonoBehaviour
{

    #region Properties
    private float speed;
    private float aliveTimer;
    #endregion

    // Use this for initialization
    void Start()
    {
        speed = 0f;
        aliveTimer = 30.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (aliveTimer > 0.0f)
            aliveTimer -= Time.deltaTime;
        else
            Destroy(this.gameObject);

        if (speed < 50f)
            speed += Time.deltaTime * 5f;

        transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
    }

    #region Msg Calls
    public void Kill()
    {
        Destroy(this.gameObject);
    }
    #endregion
}