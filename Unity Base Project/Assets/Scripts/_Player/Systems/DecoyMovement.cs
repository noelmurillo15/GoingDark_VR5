using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class DecoyMovement : MonoBehaviour
{

    #region Properties
    private float speed;
    private float aliveTimer;
    private Vector3 moveDir;
    private Transform MyTransform;
    private CharacterController m_controller;
    #endregion

    // Use this for initialization
    void Start()
    {
        speed = 0f;
        aliveTimer = 30f;
        moveDir = Vector3.zero;
        MyTransform = transform;
        Invoke("Kill", aliveTimer);
        m_controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (speed < 100f)
            speed += Time.deltaTime * 5f;

        moveDir = Vector3.zero;
        moveDir = MyTransform.TransformDirection(Vector3.forward);
        moveDir *= (speed * Time.deltaTime);
        m_controller.Move(moveDir);
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