using UnityEngine;
using System.Collections;

public class TempShipRotation : MonoBehaviour {
    private x360Controller m_GamePad;
    private Transform MyTransform;

    public float rotateSpeed;

    // Use this for initialization
    void Start () {
        m_GamePad = GamePadManager.Instance.GetController(0);
        MyTransform = transform;
        rotateSpeed = 20f;
    }

    // Update is called once per frame
    void Update () {
        MyTransform.Rotate(Vector3.up * Time.deltaTime * (rotateSpeed * m_GamePad.GetLeftStick().X));

    }
}
