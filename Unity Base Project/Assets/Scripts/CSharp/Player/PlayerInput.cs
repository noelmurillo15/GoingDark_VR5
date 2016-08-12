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
    private GameObject AmmoSwitch;
    private float elaspedtime;
    private bool isAmmoShown;

    
    #endregion


    void Start()
    {
        messageUp = false;
        controller = GamePadManager.Instance.GetController(0);
        systems = GameObject.FindGameObjectWithTag("Systems").GetComponent<SystemManager>();
        movement = GetComponent<PlayerMovement>().GetMoveData();
        Invoke("FindSystems", 2f);
        

        elaspedtime = Time.time;
        AmmoSwitch = GameObject.Find("AmmoSwitch");
        isAmmoShown = true;

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

        if (controller.GetButtonDown("A") && !messageUp)
            systems.ActivateSystem(SystemType.Emp);

        if (controller.GetButtonDown("B"))
            systems.ActivateSystem(SystemType.Decoy);

        if (controller.GetButtonDown("RightBumper"))
            systems.ActivateSystem(SystemType.Missile);

        if (controller.GetButtonDown("LeftBumper"))
            systems.ActivateSystem(SystemType.Hyperdrive);

        if (controller.GetButtonDown("Right"))
            lasers.WeaponSwap();

        if (controller.GetButtonDown("Up"))
        {
            ShowAmmo();
            missiles.WeaponSwap();
        }

        if (controller.GetRightTrigger() > 0f)
            systems.ActivateSystem(SystemType.Laser);                   

        if (isAmmoShown && elaspedtime + 5.0f < Time.time)
            DontShowAmmo();

    }

    public void MessageUp(bool up)
    {
        messageUp = up;
    }
    private void ShowAmmo()
    {
        for (int i = 0; i < AmmoSwitch.transform.childCount; i++)
            AmmoSwitch.transform.GetChild(i).gameObject.SetActive(true);
        elaspedtime = Time.time;
        isAmmoShown = true;

    }

    private void DontShowAmmo()
    {
        for (int i = 0; i < AmmoSwitch.transform.childCount; i++)
            AmmoSwitch.transform.GetChild(i).gameObject.SetActive(false);

        isAmmoShown = false;
    }

}