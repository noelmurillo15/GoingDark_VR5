using UnityEngine;
using GoingDark.Core.Enums;

public class PlayerInput : MonoBehaviour
{
    #region Properties
    private bool messageUp;
    private SystemManager systems;
    private MovementProperties movement;
    private x360Controller controller;
    #endregion


    void Start()
    {
        messageUp = false;
        controller = GamePadManager.Instance.GetController(0);
        systems = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemManager>();
        movement = GetComponent<PlayerMovement>().GetMoveData();
    }

    void Update()
    {
        if (controller.GetLeftTrigger() > 0f)
            movement.ChangeSpeed(controller.GetLeftTrigger());
        else
            movement.DecreaseSpeed();

        if (controller.GetButtonDown("X"))
            systems.ActivateSystem(SystemType.Cloak);

        if (controller.GetButtonDown("A") && !messageUp)
            systems.ActivateSystem(SystemType.Emp);

        if (controller.GetButtonDown("B"))
            systems.ActivateSystem(SystemType.Decoy);

        if (controller.GetButtonDown("RightBumper"))
            systems.ActivateSystem(SystemType.Missile);

        if (controller.GetButtonDown("LeftBumper"))
            systems.ActivateSystem(SystemType.Hyperdrive);

        if (controller.GetRightTrigger() > 0f)
            systems.ActivateSystem(SystemType.Laser);
    }

    public void MessageUp(bool up)
    {
        messageUp = up;
    }
}