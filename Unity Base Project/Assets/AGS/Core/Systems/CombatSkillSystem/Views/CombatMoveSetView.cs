using System;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterSystem;
using UnityEngine;

namespace AGS.Core.Systems.CombatSkillSystem
{
    /// <summary>
    /// MoveSetViews are "blueprints" of what combat moves and combos are allowed with the specific moveset. Weapons can share MoveSets
    /// </summary>
    [Serializable]
    public class CombatMoveSetView : ActionView
    {
        #region Public properties
        public CombatMoveSetType CombatMoveSetType;
        public Transform CombatMoveCombosContainer; // Add combo references to a separate child GameObject to this Transform
        public Transform CombatMovesContainer; // Add CombatMove references to separate child GameObjects to this Transform
        #endregion

        public CombatMoveSet CombatMoveSet;

        /// <summary>
        /// Gets the owner combat entity.
        /// </summary>
        /// <value>
        /// The owner combat entity.
        /// </value>
        protected CombatEntityBase OwnerCombatEntity
        {
            get { return CombatMoveSet.OwnerCombatEntity.Value; }
        }

        #region AGS setup
        public override void InitializeView()
        {
            CombatMoveSet = new CombatMoveSet(CombatMoveSetType);
            SolveModelDependencies(CombatMoveSet);
        }

        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            if (CombatMoveCombosContainer != null)
            {
                foreach (var combatMoveComboView in CombatMoveCombosContainer.GetComponentsInChildren<CombatMoveComboView>())
                {
                    CombatMoveSet.CombatMoveCombos.Add(combatMoveComboView.CombatMoveCombo);
                }
            }
            if (CombatMovesContainer != null)
            {
                foreach (var combatMoveView in CombatMovesContainer.GetComponentsInChildren<CombatMoveView>())
                {
                    CombatMoveSet.CombatMoves.Add(combatMoveView.CombatMove);
                }
            }

        }
        #endregion

        #region MonoBehaviours
        public override void Update()
        {
            base.Update();
            if(CombatMoveSet == null) return;
            CombatMoveSet.CombatEntityPosition.Value = CheckCombatEntityPosition();
        }
        #endregion

        #region private functions
        /// <summary>
        /// Determines the OwnerCombatEntitys current CombatMovePosition.
        /// </summary>
        /// <returns></returns>
        private CombatMovePosition CheckCombatEntityPosition()
        {
            if (OwnerCombatEntity == null)
            {
                return CombatMovePosition.Standing;
            }
            var ownerCharacter = OwnerCombatEntity as CharacterBase;
            if (ownerCharacter == null)
            {
                return CombatMovePosition.Standing;
            }

            // movable characters
            if (ownerCharacter.MovementSkills.Value != null)
            {
                if (ownerCharacter.MovementSkills.Value.Swimming.Value != null)
                {
                    if (ownerCharacter.MovementSkills.Value.Swimming.Value.SwimmingCurrentState.Value == SwimmingState.InWater
                        ||
                        ownerCharacter.MovementSkills.Value.Swimming.Value.SwimmingCurrentState.Value == SwimmingState.DoingStroke)
                    {
                        return CombatMovePosition.Swimming;    
                    }
                }
                if (!ownerCharacter.IsGrounded.Value)
                {
                    return CombatMovePosition.Airborne;
                }
                if (ownerCharacter.MovementSkills.Value.HorizontalMovement.Value != null)
                {
                    if (ownerCharacter.MovementSkills.Value.HorizontalMovement.Value.HorizontalMovementCurrentState.Value == HorizontalMovementState.Crouching)
                    {
                        return CombatMovePosition.Crouching;
                    }
                    if (ownerCharacter.MovementSkills.Value.HorizontalMovement.Value.HorizontalMovementCurrentState.Value == HorizontalMovementState.Sneaking)
                    {
                        return CombatMovePosition.Sneaking;
                    }
                }
            }
            return CombatMovePosition.Standing;
        }
        #endregion
    }


}