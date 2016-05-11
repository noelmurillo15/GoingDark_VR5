using System;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterControlSystem;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.InteractionSystem.Interactables;
using AGS.Core.Systems.SkillSystem;
using UnityEngine;

namespace AGS.Core.Systems.InteractionSystem.Base
{
    /// <summary>
    /// BaseView for interaction skills. Provides several short hand convenience properties for its child implementations
    /// </summary>
    [Serializable]
    public abstract class InteractionSkillBaseView : SkillBaseView
    {
        #region Public properties
        // Fields to be set in the editor
        public float ApproachMargin;
        public float OffsetVertical;
        public float OffsetHorizontal;

        public InteractionSkillBase InteractionSkill;

        #endregion

        private InteractionSkills _ownerInteractionSkills;


        #region convenience properties
        public InteractionSkills OwnerInteractionSkills
        {
            get
            {
                if (_ownerInteractionSkills != null) return _ownerInteractionSkills;
                if (InteractionSkill.OwnerInteractionSkills.Value != null)
                {
                    _ownerInteractionSkills = InteractionSkill.OwnerInteractionSkills.Value;
                }
                return _ownerInteractionSkills;
            }
        }
        private AdvancedCharacterBase _owncerCharacter;
        public AdvancedCharacterBase OwnerCharacter
        {
            get
            {
                if (_owncerCharacter != null) return _owncerCharacter;
                if (InteractionSkill.OwnerInteractionSkills.Value != null
                    &&
                    InteractionSkill.OwnerInteractionSkills.Value.OwnerCharacter.Value != null)
                {
                    _owncerCharacter = InteractionSkill.OwnerInteractionSkills.Value.OwnerCharacter.Value;
                }
                return _owncerCharacter;
            }
        }
        private Transform _charTransform;
        public Transform CharTransform
        {
            get
            {
                if (_charTransform != null) return _charTransform;
                if (InteractionSkill.OwnerInteractionSkills.Value != null
                    &&
                    InteractionSkill.OwnerInteractionSkills.Value.OwnerCharacter.Value != null
                    &&
                    InteractionSkill.OwnerInteractionSkills.Value.OwnerCharacter.Value.Transform != null)
                {
                    _charTransform = InteractionSkill.OwnerInteractionSkills.Value.OwnerCharacter.Value.Transform;
                }
                return _charTransform;
            }

        }
        private CharacterControllerBase _ownerCharacterController;
        public CharacterControllerBase OwnerCharacterController
        {
            get
            {
                if (_ownerCharacterController != null) return _ownerCharacterController;
                if (InteractionSkill.OwnerInteractionSkills.Value != null
                    &&
                    InteractionSkill.OwnerInteractionSkills.Value.OwnerCharacter.Value != null
                    &&
                    InteractionSkill.OwnerInteractionSkills.Value.OwnerCharacter.Value.CharacterController != null)
                {
                    _ownerCharacterController = InteractionSkill.OwnerInteractionSkills.Value.OwnerCharacter.Value.CharacterController.Value;
                }
                return _ownerCharacterController;
            }
        }
        public InteractableBase CurrentInteractableTarget
        {
            get
            {
                if (InteractionSkill.OwnerInteractionSkills.Value != null
                    &&
                    InteractionSkill.OwnerInteractionSkills.Value.CurrentInteractableTarget.Value != null)
                {
                    return InteractionSkill.OwnerInteractionSkills.Value.CurrentInteractableTarget.Value;
                }
                return null;
            }
            set { InteractionSkill.OwnerInteractionSkills.Value.CurrentInteractableTarget.Value = value; }
        }
        public InteractionVolume CurrentInteractionVolume
        {
            get
            {
                if (InteractionSkill.OwnerInteractionSkills.Value != null
                    &&
                    InteractionSkill.OwnerInteractionSkills.Value.CurrentInteractionVolume.Value != null)
                {
                    return InteractionSkill.OwnerInteractionSkills.Value.CurrentInteractionVolume.Value;
                }
                return null;
            }
        }
        #endregion

        #region AGS Setup
        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            InteractionSkill = model as InteractionSkillBase;
        }
        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);

            if (InteractionSkill == null) return;

        }
        #endregion

        #region State machine functions
        /// <summary>
        /// Called when [state enter idle].
        /// </summary>
        public abstract void OnStateEnterIdle();

        /// <summary>
        /// Called when [state enter approaching].
        /// </summary>
        public abstract void OnStateEnterApproaching();

        /// <summary>
        /// Called when [state update approaching].
        /// </summary>
        public abstract void OnStateUpdateApproaching();

        /// <summary>
        /// Called when [state enter releasing].
        /// </summary>
        public abstract void OnStateEnterReleasing();
        #endregion

        #region abstract functions
        /// <summary>
        /// Gets the target position.
        /// </summary>
        public abstract Vector3 GetTargetPosition();

        /// <summary>
        /// Moves the character towards interactable target.
        /// </summary>
        /// <param name="targetPosition">The target position.</param>
        /// <param name="smoothness">The smoothness.</param>
        public abstract void MoveTowardsInteractableTarget(Vector3 targetPosition, float smoothness);

        /// <summary>
        /// Checks where character reached the target position.
        /// </summary>
        /// <param name="targetPosition">The target position.</param>
        /// <param name="margin">The margin.</param>
        /// <returns></returns>
        public bool ReachedTargetPosition(Vector3 targetPosition, float margin)
        {
            var offset = CharTransform.position - targetPosition;
            var sqrLen = offset.sqrMagnitude;
            return sqrLen < margin * margin;
        }
        #endregion
    }
}