using System;
using System.Linq;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.DataClasses;
using AGS.Core.Enums;
using AGS.Core.Systems.MovementSystem.Base;
using AGS.Core.Systems.MovementSystem.MovementSkills.HorizontalMovement;
using AGS.Core.Classes.TimerComponents;
using UnityEngine;
using Object = UnityEngine.Object;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Base;

namespace AGS.Core.Systems.CharacterSystem
{
    

    /// <summary>
    /// Player model to be used only with a player character
    /// </summary>
    public class Player : AdvancedCharacterBase
    {
        #region Properties
        public KillableBase[] EnemiesWithinRange { get; set; } // Enemies within range

        // Subscribable properties
        public ActionProperty<bool> AutoTarget { get; private set; } // Should player auto target enemies?
        public ActionProperty<bool> IsSilent { get; private set; } // Is player detectable?

        public Action<Object, Object> CollectWeaponAction { get; set; } // Subscribe to this to get notified of weapon pick ups

        private UpdateTemporaryGameObject _autoTargetUpdater;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <param name="name">The name.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="turnSpeed">Turn speed of the character</param>
        /// <param name="skinWidth">Width of the skin.</param>
        /// <param name="skinCorrectionRays">The skin correction rays.</param>
        /// <param name="slopeLimitMoving">The slope limit moving.</param>
        /// <param name="slopeLimitSliding">The slope limit sliding.</param>
        public Player(Transform transform, string name, float speed, float turnSpeed, float skinWidth, int skinCorrectionRays, float slopeLimitMoving, float slopeLimitSliding)
            : base(transform, name, speed, turnSpeed, skinWidth, skinCorrectionRays, slopeLimitMoving, slopeLimitSliding)
        {
            AutoTarget = new ActionProperty<bool>() {  };
            AutoTarget.OnValueChanged += (sender, autoTarget) => OnAutoTargetChanged(autoTarget.Value);
            IsSilent = new ActionProperty<bool>();
        }
        #region private

        /// <summary>
        /// Called when [automatic target changed].
        /// </summary>
        /// <param name="autoTarget">if set to <c>true</c> [automatic target].</param>
        private void OnAutoTargetChanged(bool autoTarget)
        {
            if (autoTarget)
            {
                _autoTargetUpdater = ComponentExtensions.AddComponentOnEmptyChild<UpdateTemporaryGameObject>(GameManager.TemporaryTimerComponents, "Autotarget updater");
                _autoTargetUpdater.UpdateMethod = () =>
                {

                    var currentTarget = GetClosestEnemy();
                    if (Target.Value == currentTarget) return;
                    SetTarget(currentTarget);
                };
            }
            else if (_autoTargetUpdater != null)
            {
                _autoTargetUpdater.Stop();
            }
        }

        /// <summary>
        /// Get the closest enemy within range.
        /// </summary>
        /// <returns></returns>
        private KillableBase GetClosestEnemy()
        {
            return EnemiesWithinRange.FirstOrDefault();
        }
        #endregion

        #region Extra Movement notifications
        /// <summary>
        /// Called when [movement skills changed].
        /// </summary>
        /// <param name="movementSkills">The movement skills.</param>
        protected override void OnMovementSkillsChanged(MovementSkills movementSkills)
        {
            base.OnMovementSkillsChanged(movementSkills);
            if (movementSkills.HorizontalMovement.Value != null)
            {
                HandleHorizontalMovement(movementSkills.HorizontalMovement.Value);
            }
            else
            {
                movementSkills.HorizontalMovement.OnValueChanged += (sender, horizontalMovement) => HandleHorizontalMovement(horizontalMovement.Value);
            }
        }

        /// <summary>
        /// Subscribes to horizontal movement state and be silent when sneaking
        /// </summary>
        /// <param name="horizontalMovement">The horizontal movement.</param>
        private void HandleHorizontalMovement(HorizontalMovement horizontalMovement)
        {
            horizontalMovement.HorizontalMovementCurrentState.OnValueChanged += (sender, state) =>
            {
                IsSilent.Value = state.Value == HorizontalMovementState.Sneaking;
            };
        }
        #endregion

        #region public
        /// <summary>
        /// Sets the closest target.
        /// </summary>
        public virtual void SetTarget()
        {
            if (EnemiesWithinRange.Length > 0)
            {

            }
            Target.Value = EnemiesWithinRange[0];
        }

        /// <summary>
        /// Call this from a PickUpWeaponView when player picks up weapons. Notifies the PlayerView which instantiates the weapon and corresponding moveset.
        /// </summary>
        /// <param name="equipableWeaponPrefab">The equipable weapon prefab.</param>
        /// <param name="combatMoveSetPrefab">The combat move set prefab.</param>
        public void CollectWeapon(Object equipableWeaponPrefab, Object combatMoveSetPrefab)
        {
            if (CollectWeaponAction != null)
            {
                CollectWeaponAction(equipableWeaponPrefab, combatMoveSetPrefab);
            }
        }

        /// <summary>
        /// Call this from a PickUpThrowableView when player picks up throwables.
        /// </summary>
        /// <param name="throwableWeaponType">Type of the throwable weapon.</param>
        /// <param name="quantity">The quantity.</param>
        public void CollectThrowables(ThrowableWeaponType throwableWeaponType, int quantity)
        {
            var existingStash = ThrowableWeaponStashes.FirstOrDefault(x => x.ThrowableWeaponType == throwableWeaponType);

            if (existingStash != null)
            {
                // Player already have a stash of this type. Just add the quantity
                existingStash.Count.Value += quantity;
            }
            else
            {
                // Create a new stash and add it to stashes.
                var newStash = new ThrowableWeaponStash()
                {
                    Count = new ActionProperty<int>() {Value = quantity},
                    ThrowableWeaponType = throwableWeaponType
                };
                ThrowableWeaponStashes.Add(newStash);
            }
        }
        #endregion
    }
}
