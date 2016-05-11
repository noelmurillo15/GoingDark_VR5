using System;
using System.Linq;
using AGS.Core.Enums;
using AGS.Core.Systems.CharacterSystem;
using UnityEngine;
using UnityEngine.UI;

namespace AGS.Core.Classes.UIComponents
{
    /// <summary>
    /// This UI component swaps an UI Image based on ThrowableWeaponType type
    /// </summary>
    [Serializable]
    public class ThrowableWeaponTypeImage
    {
        public ThrowableWeaponType ThrowableWeaponType;
        public Sprite Sprite;
    }

    public class ActiveThrowableImageSwapper : UIScriptBase
    {
        public Image Image;
        public ThrowableWeaponTypeImage[] ThrowableWeaponTypeImages;


        public override void Awake()
        {
            base.Awake();
            Image = GetComponent<Image>();
        }

        protected override void SetupModelBindings()
        {
            base.SetupModelBindings();
            GameLevel.Player.OnValueChanged += (sender, player) => OnPlayerChanged(player.Value);
        }

        /// <summary>
        /// Called when [player changed].
        /// </summary>
        /// <param name="player">The player.</param>
        private void OnPlayerChanged(Player player)
        {

            player.ActiveThrowableType.OnValueChanged += (sender, activeTrowableType) => OnActiveThrowableChanged(activeTrowableType.Value);
        }

        /// <summary>
        /// Called when [active throwable changed].
        /// </summary>
        /// <param name="activeTrowableType">Type of the active trowable.</param>
        private void OnActiveThrowableChanged(ThrowableWeaponType activeTrowableType)
        {
            var newTypeImage = ThrowableWeaponTypeImages.FirstOrDefault(x => x.ThrowableWeaponType == activeTrowableType);
            if (newTypeImage != null) Image.sprite = newTypeImage.Sprite;
        }
    }
}
