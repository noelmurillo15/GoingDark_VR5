using AGS.Core.Base;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.CharacterSystem;
using UnityEngine;
using UnityEngine.UI;

namespace AGS.Core.Classes.UIComponents
{
    /// <summary>
    /// This abstract UI component handles one Damagable resource and displays it using an UI.Image and updates the fillAmount
    /// DamageableResourceType sets the type of resource to subscribe to. HideOnFull only shows bar when resource is damaged.
    /// </summary>
    public abstract class DamageableResourceBar : UIScriptBase
    {
        public Image ResourceBar;
        public CanvasGroup CanvasGroup;
        public DamageableResourceType ResourceType;
        public bool HideOnFull;
        protected KillableBase TargetKillable;
        private ActionProperty<bool> _resourceIsCorrect;

        private UpdateTemporaryGameObject _resourceCheckUpdater;
        protected override void SetupModelBindings()
        {
            base.SetupModelBindings();
            _resourceIsCorrect = new ActionProperty<bool>();
            GameLevel.Player.OnValueChanged += (sender, player) => OnPlayerChanged(player.Value);

            _resourceIsCorrect.OnValueChanged += (sender, x) =>
            {
                CheckVisibility();
                if (_resourceCheckUpdater != null)
                {
                    _resourceCheckUpdater.Stop();
                }
            };
        }

        /// <summary>
        /// Sets the target.
        /// </summary>
        /// <param name="killable">The killable.</param>
        protected abstract void SetTarget(KillableBase killable);

        /// <summary>
        /// Called when [player changed].
        /// </summary>
        /// <param name="player">The player.</param>
        protected abstract void OnPlayerChanged(Player player);

        /// <summary>
        /// Called when [resource changed].
        /// </summary>
        /// <param name="resource">The resource.</param>
        protected void OnResourceChanged(DamageableResource resource)
        {
            ResourceBar.fillAmount = resource.Current.Value / (float)resource.Max.Value;
            _resourceIsCorrect.Value = true;
            CheckVisibility();
            resource.Current.OnValueChanged += (sender, current) =>
                {
                    if (_resourceCheckUpdater != null)
                    {
                        _resourceCheckUpdater.Stop();
                    }
                    _resourceIsCorrect.Value = false;

                    _resourceCheckUpdater = MonoExtensions.ComponentExtensions.AddComponentOnEmptyChild<UpdateTemporaryGameObject>(GameLevelHUDView.gameObject ?? GameManager.TemporaryTimerComponents, "Resource check updater");
                    _resourceCheckUpdater.UpdateMethod = () =>
                    {
                        {
                            var targetAmount = current.Value / (float)resource.Max.Value;
                            ResourceBar.fillAmount = Mathf.Lerp(ResourceBar.fillAmount, targetAmount, 0.1f);
                            if (Mathf.Abs(ResourceBar.fillAmount - targetAmount) < 0.001f)
                            {
                                _resourceIsCorrect.Value = true;
                            }
                        }
                    };
                    CheckVisibility();
                };
        }

        /// <summary>
        /// Checks the visibility.
        /// </summary>
        private void CheckVisibility()
        {
            if (HideOnFull)
            {
                CanvasGroup.alpha = Mathf.Abs(ResourceBar.fillAmount - 1f) < 0.01f
                    ? 0
                    : 1;
            }
        }
    }
}
