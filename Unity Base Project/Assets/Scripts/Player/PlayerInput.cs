using UnityEngine;
using GoingDark.Core.Enums;

public class PlayerInput : MonoBehaviour
{
    #region Properties
    private bool messageUp;
    private SystemManager systems;
    private LaserSystem lasers;
    private MissileSystem missiles;
    private MovementProperties movement;
    private x360Controller controller;
    private MissionTracker tracker;

    #endregion


    void Start()
    {
        messageUp = false;
        controller = GamePadManager.Instance.GetController(0);
        systems = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemManager>();
        tracker = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MissionTracker>();
        movement = GetComponent<PlayerMovement>().GetMoveData();
        Invoke("FindSystems", 2f);

    }

    void FindSystems()
    {
        lasers = systems.GetSystemScript(SystemType.Laser) as LaserSystem;
        missiles = systems.GetSystemScript(SystemType.Missile) as MissileSystem;
    }

    void Update()
    {
        if (controller.GetLeftTrigger() > 0f)
            movement.ChangeSpeed(controller.GetLeftTrigger());
        else
            movement.DecreaseSpeed();

        if (controller.GetButtonDown("X"))
            systems.ActivateSystem(SystemType.Cloak);

        if (controller.GetButtonDown("B"))
            systems.ActivateSystem(SystemType.Decoy);

        if (controller.GetButtonDown("RightBumper"))
            systems.ActivateSystem(SystemType.Missile);

        if (controller.GetButtonDown("LeftBumper"))
            systems.ActivateSystem(SystemType.Hyperdrive);

        if (controller.GetButtonDown("Down") && !messageUp)
            lasers.WeaponSwap();

        if (controller.GetButtonDown("Left") && tracker != null)
            tracker.PrevMission();

        if (controller.GetButtonDown("Right") && tracker != null)
            tracker.NextMission();

        if (controller.GetButtonDown("Up") && !messageUp)
        {
            missiles.WeaponSwap();
        }

        if (controller.GetRightTrigger() > 0f)
            systems.ActivateSystem(SystemType.Laser);                   

    }

    public void MessageUp(bool up)
    {
        messageUp = up;
    }

}