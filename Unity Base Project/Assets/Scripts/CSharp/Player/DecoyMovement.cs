using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class DecoyMovement : MonoBehaviour
{

    #region Properties
    private float hp;
    private float speed;
    private float aliveTimer;
    private Vector3 moveDir;
    private Transform MyTransform;
    private CharacterController m_controller;
    #endregion

    // Use this for initialization
    void Start()
    {
        hp = 10;
        speed = 80f;
        aliveTimer = 15f;
        moveDir = Vector3.zero;
        MyTransform = transform;
        Invoke("Kill", aliveTimer);
        m_controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (speed < 100f)
            speed += Time.deltaTime * 5f;

        moveDir = MyTransform.TransformDirection(Vector3.forward);
        moveDir *= (speed * Time.deltaTime);
        m_controller.Move(moveDir);
    }

    #region Msg Calls
    public void Hit()
    {
        hp--;
        if (hp <= 0)
            Kill();
    }
    public void Kill()
    {
        Destroy(gameObject);
    }
    #endregion
}