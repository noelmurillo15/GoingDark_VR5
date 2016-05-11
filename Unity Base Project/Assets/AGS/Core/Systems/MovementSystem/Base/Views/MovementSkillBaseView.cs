using System;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterControlSystem;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.SkillSystem;
using UnityEngine;

namespace AGS.Core.Systems.MovementSystem.Base
{
    /// <summary>
    /// BaseView for movement skills. Provides several short hand convenience properties for its child implementations
    /// </summary>
    [Serializable]
    public abstract class MovementSkillBaseView : SkillBaseView
    {
        #region Public properties
        public MovementSkillBase MovementSkill;

        #endregion

        /// <summary>
        /// Convenience property. Gets the owner character.
        /// </summary>
        /// <value>
        /// The owner character.
        /// </value>
        public CharacterBase OwnerCharacter
        {
            get
            {
                if (_owncerCharacter != null) return _owncerCharacter;
                if (MovementSkill.OwnerMovementSkills.Value != null
                    &&
                    MovementSkill.OwnerMovementSkills.Value.OwnerCharacter.Value != null)
                {
                    _owncerCharacter = MovementSkill.OwnerMovementSkills.Value.OwnerCharacter.Value;
                }
                return _owncerCharacter;
            }
        }
        private CharacterBase _owncerCharacter;

        /// <summary>
        /// Convenience property. Gets the character transform.
        /// </summary>
        /// <value>
        /// The character transform.
        /// </value>
        public Transform CharTransform
        {
            get
            {
                if (_charTransform != null) return _charTransform;
                if (MovementSkill.OwnerMovementSkills.Value != null
                    &&
                    MovementSkill.OwnerMovementSkills.Value.OwnerCharacter.Value != null
                    &&
                    MovementSkill.OwnerMovementSkills.Value.OwnerCharacter.Value.Transform != null)
                {
                    _charTransform = MovementSkill.OwnerMovementSkills.Value.OwnerCharacter.Value.Transform;
                }
                return _charTransform;
            }

        }
        private Transform _charTransform;

        /// <summary>
        /// Convenience property. Gets the owner character controller.
        /// </summary>
        /// <value>
        /// The owner character controller.
        /// </value>
        protected CharacterControllerBase OwnerCharacterController
        {
            get
            {
                if (_ownerCharacterController != null) return _ownerCharacterController;
                if (MovementSkill.OwnerMovementSkills.Value != null
                    &&
                    MovementSkill.OwnerMovementSkills.Value.OwnerCharacter.Value != null
                    &&
                    MovementSkill.OwnerMovementSkills.Value.OwnerCharacter.Value.CharacterController.Value != null)
                {
                    _ownerCharacterController = MovementSkill.OwnerMovementSkills.Value.OwnerCharacter.Value.CharacterController.Value;
                }
                return _ownerCharacterController;
            }
        }
        private CharacterControllerBase _ownerCharacterController;

        #region AGS Setup
        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            MovementSkill = model as MovementSkillBase;
            if (MovementSkill == null) return;

        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);

            if (MovementSkill == null) return;
        }
        #endregion
    }
}