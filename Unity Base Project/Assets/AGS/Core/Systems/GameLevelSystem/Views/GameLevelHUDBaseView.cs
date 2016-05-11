using System;
using System.Linq;
using AGS.Core.Base;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.GUISystem;
using UnityEngine;

namespace AGS.Core.Systems.GameLevelSystem
{
    /// <summary>
    /// The GameLevelHUDBaseView view is an abstract class. Inherit from this to create a game specific HUD. Instance HUD views are naturally positioned on an immediate child of the GameManager.
    /// </summary>
    [Serializable]
    public abstract class GameLevelHUDBaseView : MonoBehaviour
    {
        #region Public properties
        // Fields and references to be set in the editor
        public Vector3 HUDRotation;
        public GameObject OverlayTextContainer;
        public GameObject FloatingTextContainer;
        public GameLevelBaseView GameLevelView;
        public GameLevel GameLevel;
        public Vector3 HUDElementsRotation;
        public ActionList<OverlayText> OverlayTexts { get; private set; }
        public ActionList<FloatingText> FloatingTexts { get; private set; }

        public Action HUDInitialized { get; set; } // When we get notified that GameLevelBaseView is ready, we use this Action to notify any subscribing UI components that also the HUDView is ready.
        public static Action OnShowPauseMenuAction; // For notifying of game pause
        #endregion


        void Start()
        {
            if (GameLevelView == null) return;
            OverlayTexts = new ActionList<OverlayText>();
            FloatingTexts = new ActionList<FloatingText>();
            GameLevelView.ViewReady.OnValueChanged += (sender, viewReady) =>
            {
                if (!viewReady.Value) return;
                // GameLevelBaseView is ready. Run SetupModelBindings that initialized the HUD and notify subscribers that HUD is ready.
                SetupModelBindings(); 
                if (HUDInitialized != null)
                {
                    HUDInitialized();    
                }
                
            };
        }

        protected virtual void SetupModelBindings()
        {
            if (GameLevelView == null) return;
            GameLevel = GameLevelView.GameLevel;
            if (GameLevel == null) return;
            GameLevel.Player.OnValueChanged += (sender, player) => PlayerChanged(player.Value);
            if (GameLevel.Enemies.Any())
            {
                foreach (var enemy in GameLevel.Enemies)
                {
                    EnemyAdded(enemy);
                }
            }
            GameLevel.Enemies.ListItemAdded += EnemyAdded;

            OverlayTexts.ListItemAdded += OverlayTextAdded;
            OverlayTexts.ListItemRemoved += x => x.DestroyModel();
            FloatingTexts.ListItemAdded += FloatingTextAdded;
            FloatingTexts.ListItemRemoved += x => x.DestroyModel();
        }
        
        /// <summary>
        /// Called when player changed.
        /// </summary>
        /// <param name="player">The player.</param>
        private void PlayerChanged(Player player)
        {
            if (!GameSettings.ShowCombatText) return;

            // Create green floating text for positive added supernatural effects, red for negative
            player.ActiveSuperNaturalEffects.ListItemAdded += superNaturalEffect =>
            {
                var spawnPosition = Vector3.zero;
                if (player.Transform != null)
                {
                    spawnPosition = SpawnPosition(player);
                }
                CreateFloatingText(InsertSpacesBeforeCapitalLetters(superNaturalEffect.EffectType.ToString()), Color.green, spawnPosition);
            };

            // Create green floating text for negative removed supernatural effects, red for positive
            player.ActiveSuperNaturalEffects.ListItemRemoved += superNaturalEffect =>
            {
                var spawnPosition = Vector3.zero;
                if (player.Transform != null)
                {
                    spawnPosition = SpawnPosition(player);
                }
                CreateFloatingText(InsertSpacesBeforeCapitalLetters(superNaturalEffect.EffectType.ToString()), Color.red, spawnPosition);
            };

            // Create green floating text for positive added movement effects, red for negative
            player.ActiveMovementEffects.ListItemAdded += movementEffect =>
            {
                var spawnPosition = Vector3.zero;
                if (player.Transform != null)
                {
                    spawnPosition = SpawnPosition(player);
                }
                CreateFloatingText(InsertSpacesBeforeCapitalLetters(movementEffect.EffectType.ToString()), movementEffect.Strength < 0f ? Color.red : Color.green, spawnPosition);
            };

            // Create green floating text for negative removed movement effects, red for positive
            player.ActiveMovementEffects.ListItemRemoved += movementEffect =>
            {
                var spawnPosition = Vector3.zero;
                if (player.Transform != null)
                {
                    spawnPosition = SpawnPosition(player);
                }
                CreateFloatingText(InsertSpacesBeforeCapitalLetters(movementEffect.EffectType.ToString()), movementEffect.Strength < 0f ? Color.green : Color.red, spawnPosition);
            };

            if (player.Resources == null) return;
            var playerHealth = player.Resources.FirstOrDefault(x => x.ResourceType.Value == DamageableResourceType.Health);
            if (playerHealth != null)
            {
                SetupCombatTextCreation(player, playerHealth);
            }
            else
            {
                player.Resources.ListItemAdded += characterResource =>
                {
                    if (characterResource.ResourceType.Value == DamageableResourceType.Health)
                    {
                        SetupCombatTextCreation(player, characterResource);
                    }
                };
            }
        }

        /// <summary>
        /// Finds spawn position for floating texts.
        /// </summary>
        /// <param name="killable">The killable.</param>
        /// <returns></returns>
        private static Vector3 SpawnPosition(KillableBase killable)
        {
            var spawnPosition = killable.Transform.position;
            var offset = new Vector3(0f, killable.Transform.GetComponent<CapsuleCollider>().height,
                0f);
            spawnPosition += offset;
            return spawnPosition;
        }

        /// <summary>
        /// ListItem notification. Enemy was added.
        /// </summary>
        /// <param name="enemy">The enemy.</param>
        private void EnemyAdded(Enemy enemy)
        {
            if (!GameSettings.ShowCombatText) return;
            var healthResource = enemy.Resources.FirstOrDefault(x => x.ResourceType.Value == DamageableResourceType.Health);
            if (healthResource != null)
            {
                SetupCombatTextCreation(enemy, healthResource);
            }
        }

        /// <summary>
        /// Setups the combat text creation.
        /// Creates green floating text for positive resource effects, red for negative.
        /// </summary>
        /// <param name="killable">The killable.</param>
        /// <param name="damageableResource">The damageable resource.</param>
        private void SetupCombatTextCreation(KillableBase killable, DamageableResource damageableResource)
        {
            var previous = damageableResource.Current.Value;
            damageableResource.Current.OnValueChanged += (sender, current) =>
                {
                    if (previous == current.Value) return;
                    var spawnPosition = Vector3.zero;
                    if (killable.Transform != null)
                    {
                        spawnPosition = SpawnPosition(killable);
                    }
                    string text;
                    Color color;
                    if (previous < current.Value)
                    {
                        text = string.Format("+{0}", current.Value - previous);
                        color = Color.green;
                    }
                    else
                    {
                        text = string.Format("{0}", current.Value - previous);
                        color = Color.red;
                    }

                    CreateFloatingText(text, color, spawnPosition);
                    previous = current.Value;
                };

        }

        /// <summary>
        /// Create the floating text and set up a timer for its removal
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="color">The color.</param>
        /// <param name="spawnPosition">The spawn position.</param>
        private void CreateFloatingText(string text, Color color, Vector3 spawnPosition)
        {
            var floatingText = new FloatingText(text, color, 1f, "FloatingTextElement", spawnPosition, HUDElementsRotation);
            FloatingTexts.Add(floatingText);
            var timerComponent = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(gameObject);
            timerComponent.TimerMethod = () => FloatingTexts.Remove(floatingText);
            timerComponent.Invoke(floatingText.LifetimeSeconds.Value);
        }

        /// <summary>
        /// Inserts a space before capital letters in string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string InsertSpacesBeforeCapitalLetters(string text)
        {
            var spacedText = text;
            var spacesInserted = 0;
            for (var i = 1; i < text.Length; i++)
                if (Char.IsUpper(text[i]))
                {
                    spacedText = spacedText.Insert(i + spacesInserted, " ");
                    spacesInserted++;
                }

            return spacedText;
        }

        #region abstract functions
        /// <summary>
        /// ListItem notification. OverlayText was added.
        /// </summary>
        /// <param name="overlayText">The overlayText.</param>
        protected abstract void OverlayTextAdded(OverlayText overlayText);

        /// <summary>
        /// ListItem notification. FloatingText was added.
        /// </summary>
        /// <param name="floatingText">The floating text.</param>
        protected abstract void FloatingTextAdded(FloatingText floatingText);
        #endregion
    }
}