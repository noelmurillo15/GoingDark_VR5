using AGS.Core.Systems.CharacterSystem;
using UnityEngine;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// ViewScript that handles players NoiseVolume
    /// </summary>
    public class PlayerNoiseVolume : ViewScriptBase
    {
        public SphereCollider NoiseVolume;
        protected PlayerBaseView PlayerBaseView;
        public Player Player;


        protected override void SetupModelBindings()
        {
            if (ViewReference != null)
            {
                PlayerBaseView = ViewReference as PlayerBaseView;
                if (PlayerBaseView == null) return;

                Player = PlayerBaseView.Player;

            }
            if (Player == null) return;
            Player.IsSilent.OnValueChanged += (sender, isSilent) => NoiseVolume.enabled = !isSilent.Value;
        }
    }
}
