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
        aliveTimer = 30f;
        Invoke("Kill", aliveTimer);
    }

    // Update is called once per frame
    void Update()
    {
        if (speed < 100f)
            speed += Time.deltaTime * 5f;

        transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
    }

    #region Msg Calls
    public void Kill()
    {
        //GameObject.Find("Enemies").SendMessage("TargetDestroyed");
        Destroy(gameObject);
    }
    public void SetSpeed(float _speed)
    {
        speed = _speed;
    }
    #endregion
}