using System.Linq;
using AGS.Core.Base;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.CombatSkillSystem;
using AGS.Core.Systems.InteractionSystem.Base;
using UnityEngine;

namespace AGS.Core.Systems.CharacterSystem
{
    /// <summary>
    /// Advanced characters are characters with interactionskills
    /// </summary>
    public class AdvancedCharacterBase : CharacterBase
    {
        // Subscribable properties
        public ActionProperty<InteractionSkills> InteractionSkills { get; set; } // Owned interaction skills

        /// <summary>
        /// Initializes a new instance of the <see cref="AdvancedCharacterBase"/> class.
        /// </summary>
        /// <param name="transform">The CharacterBase transform.</param>
        /// <param name="name">CharacterBase name. Is instantiating ragdolls upon death, this need to match Ragdoll prefix. A Killable with the name John will try to instantiate JohnRagdoll</param>
        /// <param name="speed">General speed of the character</param>
        /// <param name="turnSpeed">Turn speed of the character</param>
        /// <param name="skinWidth">SkinWidth determines how far out from Character Collider raycasts that determines environment detection should go</param>
        /// <param name="skinCorrectionRays">How many rays should be used for detecting environment</param>
        /// <param name="slopeLimitMoving">Slope angle treshold in degrees before character begins to slide on slopes</param>
        /// <param name="slopeLimitSliding">Slope angle treshold in degress that determines if character should crouch or start manual sliding</param>
        public AdvancedCharacterBase(Transform transform, string name, float speed, float turnSpeed, float skinWidth, int skinCorrectionRays, float slopeLimitMoving, float slopeLimitSliding)
            : base(transform, name, speed, turnSpeed, skinWidth, skinCorrectionRays, slopeLimitMoving, slopeLimitSliding)
        {
            InteractionSkills = new ActionProperty<InteractionSkills>();
            InteractionSkills.OnValueChanged += (sender, interactionSkills) => OnInteractionSkillsChanged(interactionSkills.Value);
        }

        #region private functions
        /// <summary>
        /// Called when [interaction skills changed].
        /// </summary>
        /// <param name="interactionSkills">The interaction skills.</param>
        private void OnInteractionSkillsChanged(InteractionSkills interactionSkills)
        {
            if (InteractionSkills.Value == null) return;
            interactionSkills.OwnerCharacter.Value = this;
            interactionSkills.LedgeClimbing.OnValueChanged += (sender, ledgeClimbing) =>
            {
                ledgeClimbing.Value.LedgeClimbingCurrentState.OnValueChanged +=
                    (_, state) => OnLedgeClimbingStateChanged(state.Value);
            };
            interactionSkills.Swinging.OnValueChanged += (sender, swinging) =>
            {
                swinging.Value.SwingingCurrentState.OnValueChanged += (_, state) => OnSwingingStateChanged(state.Value);
            };
            interactionSkills.LadderClimbing.OnValueChanged += (sender, ladderClimbing) =>
            {
                ladderClimbing.Value.LadderClimbingCurrentState.OnValueChanged +=
                    (_, state) => OnLadderClimbingStateChanged(state.Value);
            };
            interactionSkills.ObjectMovement.OnValueChanged += (sender, objectMovement) =>
            {
                objectMovement.Value.ObjectMovementCurrentState.OnValueChanged +=
                    (_, state) => OnObjectMovementStateChanged(state.Value);
            };

        }
        #endregion

        #region insteraction skill notifications
        /// <summary>
        /// Called when [ledge climbing state changed].
        /// Enables and disables other skills based on LedgeClimbingState.
        /// </summary>
        /// <param name="ledgeClimbingState">State of the ledge climbing.</param>
        private void OnLedgeClimbingStateChanged(LedgeClimbingState ledgeClimbingState)
        {

            if (ledgeClimbingState == LedgeClimbingState.Approaching)
            {
                MovementAndJumpingDirectionLock();
                DisableCombatSkills();
            }
            if (ledgeClimbingState == LedgeClimbingState.Idle)
            {
                MovementAndJumpingDirectionUnLock();
                EnableCombatSkills();
            }
            if (ledgeClimbingState == LedgeClimbingState.Jumping)
            {
                MovementSkills.Value.HorizontalMovement.Value.SkillTransitionToStateDisable();

                var skillEnableTimer = Classes.MonoExtensions.ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(GameManager.TemporaryTimerComponents, "Skill enable timer");
                skillEnableTimer.TimerMethod = () =>
                {
                    DirectionLocked.Value = false;
                    MovementSkills.Value.HorizontalMovement.Value.SkillTransitionToStateEnable();
                    EnableCombatSkills();
                };
                skillEnableTimer.Invoke(0.5f);
            }
        }

        /// <summary>
        /// Called when [swinging state changed].
        ///  Enables and disables other skills based on SwingingState.
        /// </summary>
        /// <param name="swingingState">State of the swinging.</param>
        private void OnSwingingStateChanged(SwingingState swingingState)
        {

            if (swingingState == SwingingState.Approaching)
            {
                MovementAndJumpingDirectionLock();
                DisableCombatSkills();
                DisableSlidingSkill();
            }
            if (swingingState == SwingingState.Idle)
            {
                MovementAndJumpingDirectionUnLock();
                EnableCombatSkills();
                EnableSlidingSkill();
            }
            if (swingingState == SwingingState.Jumping)
            {
                var skillEnableTimer = Classes.MonoExtensions.ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(GameManager.TemporaryTimerComponents, "Skill enable timer");
                skillEnableTimer.TimerMethod = () =>
                {
                    DirectionLocked.Value = false;
                    MovementSkills.Value.HorizontalMovement.Value.SkillTransitionToStateEnable();
                    MovementSkills.Value.VerticalMovement.Value.SkillTransitionToStateEnable();
                    EnableCombatSkills();
                };
                skillEnableTimer.Invoke(0.1f);
            }
        }

        /// <summary>
        /// Called when [ladder climbing state changed].
        /// Enables and disables other skills based on LadderClimbingState.
        /// </summary>
        /// <param name="ladderClimbingState">State of the ladder climbing.</param>
        private void OnLadderClimbingStateChanged(LadderClimbingState ladderClimbingState)
        {

            if (ladderClimbingState == LadderClimbingState.Approaching)
            {
                MovementAndJumpingDirectionLock();
                if (ThrowingSkill.Value != null)
                {
                    ThrowingSkill.Value.SkillTransitionToStateDisable();
                }
                DisableCombatSkills();
            }
            if (ladderClimbingState == LadderClimbingState.Idle)
            {
                MovementAndJumpingDirectionUnLock();
                if (ThrowingSkill.Value != null)
                {
                    ThrowingSkill.Value.SkillTransitionToStateEnable();
                }
                
                EnableCombatSkills();
            }
            if (ladderClimbingState == LadderClimbingState.Jumping)
            {
                var skillEnableTimer = Classes.MonoExtensions.ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(GameManager.TemporaryTimerComponents, "Skill enable timer");
                skillEnableTimer.TimerMethod = () =>
                {
                    MovementAndJumpingDirectionUnLock();
                    if (ThrowingSkill.Value != null)
                    {
                        ThrowingSkill.Value.SkillTransitionToStateEnable();
                    }
                    EnableCombatSkills();
                };
                skillEnableTimer.Invoke(0.1f);
            }
        }

        /// <summary>
        /// Called when [object movement state changed].
        /// Enables and disables other skills based on ObjectMovementState.
        /// </summary>
        /// <param name="objectMovementState">State of the object movement.</param>
        private void OnObjectMovementStateChanged(ObjectMovementState objectMovementState)
        {

            if (objectMovementState == ObjectMovementState.Grabbing)
            {
                MovementAndJumpingDirectionLock();
                if (ThrowingSkill.Value != null)
                {
                    ThrowingSkill.Value.SkillTransitionToStateDisable();
                }
                
                DisableCombatSkills();
            }
            else if (objectMovementState == ObjectMovementState.Carrying)
            {
                MovementAndJumpingDirectionLock();
                if (ThrowingSkill.Value != null)
                {
                    ThrowingSkill.Value.SkillTransitionToStateDisable();
                }
                DisableCombatSkills();
                var skillEnableTimer = Classes.MonoExtensions.ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(GameManager.TemporaryTimerComponents, "Movement enable timer");
                skillEnableTimer.TimerMethod = MovementAndJumpingDirectionUnLock;
                skillEnableTimer.Invoke(0.5f);
            }
            else if (objectMovementState == ObjectMovementState.Idle)
            {
                var skillEnableTimer = Classes.MonoExtensions.ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(GameManager.TemporaryTimerComponents, "Movement enable timer");
                skillEnableTimer.TimerMethod = () =>
                {
                    MovementAndJumpingDirectionUnLock();
                    if (ThrowingSkill.Value != null)
                    {
                        ThrowingSkill.Value.SkillTransitionToStateEnable();
                    }
                    EnableCombatSkills();
                };
                skillEnableTimer.Invoke(0.25f);
            }
        }
        #endregion

        #region extra movementskill notifications
        /// <summary>
        /// Enables all interaction skills.
        /// </summary>
        protected void EnableInteractionSkills()
        {
            if (InteractionSkills == null || InteractionSkills.Value == null) return;

            if (InteractionSkills.Value.LadderClimbing.Value != null)
            {
                InteractionSkills.Value.LadderClimbing.Value.SkillTransitionToStateEnable();
            }
            if (InteractionSkills.Value.LedgeClimbing.Value != null)
            {
                InteractionSkills.Value.LedgeClimbing.Value.SkillTransitionToStateEnable();
            }
            if (InteractionSkills.Value.ObjectMovement.Value != null)
            {
                InteractionSkills.Value.ObjectMovement.Value.SkillTransitionToStateEnable();
            }
            if (InteractionSkills.Value.Swinging.Value != null)
            {
                InteractionSkills.Value.Swinging.Value.SkillTransitionToStateEnable();
            }
            if (InteractionSkills.Value.SwitchInteraction.Value != null)
            {
                InteractionSkills.Value.SwitchInteraction.Value.SkillTransitionToStateEnable();
            }
        }

        /// <summary>
        /// Disables all interaction skills.
        /// </summary>
        protected void DisableInteractionSkills()
        {
            if (InteractionSkills == null || InteractionSkills.Value == null) return;

            if (InteractionSkills.Value.LadderClimbing.Value != null)
            {
                InteractionSkills.Value.LadderClimbing.Value.SkillTransitionToStateDisable();
            }
            if (InteractionSkills.Value.LedgeClimbing.Value != null)
            {
                InteractionSkills.Value.LedgeClimbing.Value.SkillTransitionToStateDisable();
            }
            if (InteractionSkills.Value.ObjectMovement.Value != null)
            {
                InteractionSkills.Value.ObjectMovement.Value.SkillTransitionToStateDisable();
            }
            if (InteractionSkills.Value.Swinging.Value != null)
            {
                InteractionSkills.Value.Swinging.Value.SkillTransitionToStateDisable();
            }
            if (InteractionSkills.Value.SwitchInteraction.Value != null)
            {
                InteractionSkills.Value.SwitchInteraction.Value.SkillTransitionToStateDisable();
            }
        }

        /// <summary>
        /// Called when [swimming state changed].
        /// Disable all interaction skills when swimming
        /// </summary>
        /// <param name="swimmingState">State of the swimming.</param>
        protected override void OnSwimmingStateChanged(SwimmingState swimmingState)
        {
            base.OnSwimmingStateChanged(swimmingState);
            if (swimmingState == SwimmingState.OutOfWater)
            {
                EnableInteractionSkills();
            }
            else
            {
                DisableInteractionSkills();
            }
        }

        /// <summary>
        /// Called when [throwing skill changed].
        /// Throwingskill notification. Do not enable combat skills if busy interacting
        /// </summary>
        /// <param name="throwingSkill">The throwing skill.</param>
        protected override void OnThrowingSkillChanged(ThrowingSkill throwingSkill)
        {
            throwingSkill.OwnerCharacter.Value = this;
            throwingSkill.BeginThrowAction += DisableCombatSkills;
            throwingSkill.EndThrowAction += () =>
            {
                if (InteractionSkills.Value != null)
                {
                    if (!IsBusyInteracting())
                    {
                        EnableCombatSkills();
                    }
                }
                else
                {
                    EnableCombatSkills();
                }

            };
            throwingSkill.ReleaseThrowableAction += (throwabletype, throwingType, throwingSpeed) => CalculateThrowablesLeft();
        }

        /// <summary>
        /// Called when [active combat move set changed].
        /// CombatMoveSet notification. Do not enable new combatMoveSet if busy interacting
        /// </summary>
        /// <param name="combatMoveSet">The combat move set.</param>
        protected override void OnActiveCombatMoveSetChanged(CombatMoveSet combatMoveSet)
        {
            if (combatMoveSet == null) return;
            foreach (var moveSet in CombatMoveSets.Where(x => x != combatMoveSet))
            {
                moveSet.Disable();
            }
            if (InteractionSkills.Value != null)
            {
                if (!IsBusyInteracting())
                {
                    combatMoveSet.Enable();
                }
            }
            else
            {
                combatMoveSet.Enable();
            }
        }

        /// <summary>
        /// Check to see whether advanced character is interacting with something
        /// </summary>
        /// <returns></returns>
        private bool IsBusyInteracting()
        {
            bool busyInteracting;
            if (InteractionSkills.Value.LedgeClimbing.Value != null
                &&
                InteractionSkills.Value.LedgeClimbing.Value.LedgeClimbingCurrentState.Value != LedgeClimbingState.Idle)
            {
                busyInteracting = true;
            }
            else if (InteractionSkills.Value.Swinging.Value != null
                     &&
                     InteractionSkills.Value.Swinging.Value.SwingingCurrentState.Value != SwingingState.Idle)
            {
                busyInteracting = true;
            }
            else if (InteractionSkills.Value.ObjectMovement.Value != null
                     &&
                     InteractionSkills.Value.ObjectMovement.Value.ObjectMovementCurrentState.Value != ObjectMovementState.Idle)
            {
                busyInteracting = true;
            }
            else if (InteractionSkills.Value.LadderClimbing.Value != null
                     &&
                     InteractionSkills.Value.LadderClimbing.Value.LadderClimbingCurrentState.Value != LadderClimbingState.Idle)
            {
                busyInteracting = true;
            }
            else if (InteractionSkills.Value.SwitchInteraction.Value != null
                     &&
                     InteractionSkills.Value.SwitchInteraction.Value.SwitchInteractionCurrentState.Value !=
                     SwitchInteractionState.Idle)
            {
                busyInteracting = true;
            }
            else
            {
                busyInteracting = false;
            }
            return busyInteracting;
        }
        #endregion
    }
}
