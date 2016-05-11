using AGS.Core.Systems.WeaponSystem;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// AnimEventListeners should be used to recieve events raised by mecanim animations.
    /// Used as a bridge to connect animations with GameView
    /// </summary>
    public abstract class EquipableWeaponAnimEventListenerBase : ViewScriptBase
    {
        protected EquipableWeaponBaseView EquipableWeaponBaseView;
        protected EquipableWeaponBase EquipableWeapon;

        protected override void SetupModelBindings()
        {
            if (ViewReference != null)
            {
                EquipableWeaponBaseView = ViewReference as EquipableWeaponBaseView;
                if (EquipableWeaponBaseView == null) return;

                EquipableWeapon = EquipableWeaponBaseView.EquipableWeapon;

            }
            if (EquipableWeapon == null) return;
        }
    }
}
