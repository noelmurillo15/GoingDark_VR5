using System;
using System.Collections.Generic;
using System.Linq;
using AGS.Core.Base;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterSystem;

namespace AGS.Core.Systems.CombatSkillSystem
{
    /// <summary>
    /// CombatMoveSet handles enabling and disabling their associated CombatMoves depending on current combat entity position and current combo chain.
    /// Note: If two or more combat moves in the same set are enabled at the same time, and they also share the same input key, it will be uncertain which move gets executed. Easily avoided by making sure each move is unique in some way
    /// </summary>
    public class CombatMoveSet : ActionModel
    {
        #region Properties
        // Constructor properties
        public CombatMoveSetType CombatMoveSetType { get; private set; }

        // Subscribable properties
        public ActionProperty<CombatEntityBase> OwnerCombatEntity { get; set; }
        public ActionProperty<bool> Enabled { get; private set; } // This will be false if OwnerCombatEnity switches to a weapon with another MoveSet.
        public ActionProperty<int> CombosExecuted { get; private set; }
        public ActionProperty<CombatMove> ActiveCombatMove { get; private set; } // The CombatMove that is executing right now. If none, this will be null
        public ActionProperty<CombatMovePosition> CombatEntityPosition { get; private set; }
        public ActionList<CombatMove> CombatMoves { get; private set; }
        public ActionList<CombatMoveCombo> CombatMoveCombos { get; private set; }

        // Subscribe to this action to get notified that a combo timer has run out
        public Action ComboTimeoutAction { get; set; }

        // private temporary lists for calculating available combat moves
        private List<CombatMove> _inactiveCombatMoves = new List<CombatMove>();
        private readonly List<CombatMove> _activeCombatMoves = new List<CombatMove>();
        private TimerTemporaryGameObject _comboTimer;
        
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="CombatMoveSet"/> class.
        /// </summary>
        /// <param name="combatMoveSetType">Type of the combat move set.</param>
        public CombatMoveSet(CombatMoveSetType combatMoveSetType)
        {
            CombatMoveSetType = combatMoveSetType;
            OwnerCombatEntity = new ActionProperty<CombatEntityBase>();
            OwnerCombatEntity.OnValueChanged += (sender, combatEntity) => OnOwnerCombatEntityChanged(combatEntity.Value);
            Enabled = new ActionProperty<bool>();
            CombosExecuted = new ActionProperty<int>();
            ActiveCombatMove = new ActionProperty<CombatMove>();
            CombatEntityPosition = new ActionProperty<CombatMovePosition>();
            CombatEntityPosition.OnValueChanged += (sender, position) =>
            {
                if (Enabled.Value)
                {
                    // OwnerCombatEnity changed combat position - update all moves
                    UpdateCombatMoves();
                }
            };
            CombatMoves = new ActionList<CombatMove>();
            CombatMoves.ListItemAdded += CombatMoveAdded;
            CombatMoveCombos = new ActionList<CombatMoveCombo>();
        }

        #region private functions
        /// <summary>
        /// List item notification. Combat move was added.
        /// </summary>
        /// <param name="combatMoveAdd">The combat move add.</param>
        private void CombatMoveAdded(CombatMove combatMoveAdd)
        {
            combatMoveAdd.OwnerCombatMoveSet.Value = this;
            combatMoveAdd.IsEnabled.OnValueChanged += (sender, isEnabled) => OnSkillStateChanged(isEnabled.Value);
            combatMoveAdd.CombatSkillCurrentState.OnValueChanged += (sender, state) => OnCombatMoveStateChanged(combatMoveAdd, state.Value);
        }

        /// <summary>
        /// Called when [owner combat entity changed].
        /// </summary>
        /// <param name="combatEntity">The combat entity.</param>
        private void OnOwnerCombatEntityChanged(CombatEntityBase combatEntity)
        {
            // Some skills require targets. So we need to update combat moves if target changes or if targetIsInRange changes
            combatEntity.Target.OnValueChanged += (sender, value) => UpdateCombatMoves();
            combatEntity.TargetIsInRange.OnValueChanged += (sender, value) => UpdateCombatMoves();
        }

        /// <summary>
        /// Called when [skill state changed].
        /// </summary>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        private void OnSkillStateChanged(bool isEnabled)
        {
            if (isEnabled)
            {
                // If all combat moves are enabled - also enable the combat move set
                if (CombatMoves.All(x => x.IsEnabled.Value))
                {
                    Enable();
                }                
            }
        }

        /// <summary>
        /// Called when [combat move state changed].
        /// </summary>
        /// <param name="combatMove">The combat move.</param>
        /// <param name="state">The CombatSkillState.</param>
        private void OnCombatMoveStateChanged(CombatMove combatMove, CombatSkillState state)
        {
            if (state == CombatSkillState.Charging
            ||
            state == CombatSkillState.Firing
            ||
            state == CombatSkillState.SustainedFiring)
            {
                if (ActiveCombatMove.Value == combatMove)
                {
                    return;
                }
                if (_comboTimer != null)
                {
                    _comboTimer.FinishTimer(); // new combat move was started before timer ran out
                }
                
                ActiveCombatMove.Value = combatMove;
                DisableOtherCombatMoves(combatMove);
            }
            if (state == CombatSkillState.Recharing)
            {
                if (combatMove.CombatMoveType == CombatMoveType.SingleHit || combatMove.CombatMoveType == CombatMoveType.ComboFinisher)
                {
                    CombosExecuted.Value = 0;
                    UpdateCombatMoves();                    
                    return; // we do not need to calculate anthing else if no combo chain is possible
                }

                // Continue combo chain
                CombosExecuted.Value++;
                UpdateCombatMoves();

                // set up the combo time window
                _comboTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(GameManager.TemporaryTimerComponents, "Combo Timer");
                _comboTimer.TimerMethod = () =>
                {
                    if (ActiveCombatMove.Value == null
                        ||
                            (ActiveCombatMove.Value.CombatSkillCurrentState.Value == CombatSkillState.Idle
                            ||
                            ActiveCombatMove.Value.CombatSkillCurrentState.Value == CombatSkillState.Recharing))
                    {
                        ComboTimeout();
                    }
                };
                _comboTimer.Invoke(combatMove.ComboChainTimer);
                TimerComponents.Add(_comboTimer);
            }
            if (state == CombatSkillState.Idle)
            {
                ActiveCombatMove.Value = null;
            }
        }

        /// <summary>
        /// Updates the combat moves.
        ///  This functions is responsible for enabling and disabling all combat moves in the set in any given moment
        /// </summary>
        private void UpdateCombatMoves()
        {
            if (!Enabled.Value) return;
            _activeCombatMoves.Clear();
            _inactiveCombatMoves.Clear();
            if (CombosExecuted.Value == 0)
            {
                ResetCombatMoves(); // reset all
            }
            else
            {
                foreach (var combo in CombatMoveCombos)
                {
                    if (combo.SkillSequence.Length <= CombosExecuted.Value)
                        continue; // combo is shorter than current combo execution
                    var activeCombatSkill = CombatMoves.FirstOrDefault(x => x.SkillName == combo.SkillSequence[CombosExecuted.Value]);
                    if (activeCombatSkill == null)
                        continue; // couldnt find skill with given name
                    if (_activeCombatMoves.Contains(activeCombatSkill))
                        continue; // skill was already added to activeSkills


                    // Now we need to check if skill is valid for the characters position at this moment
                    if (CombatEntityPosition.Value != activeCombatSkill.RequiredPosition)
                    {
                        continue;
                    }
                    // Passed all checks. Add skill to _activeCombatMoves
                    _activeCombatMoves.Add(activeCombatSkill);
                }
                _inactiveCombatMoves = CombatMoves.Except(_activeCombatMoves).ToList();

                // enable & disable combat skills
                foreach (var combatMove in _activeCombatMoves)
                {
                    combatMove.SkillTransitionToStateEnable();
                }
                foreach (var combatMove in _inactiveCombatMoves)
                {
                    combatMove.SkillTransitionToStateDisable();
                }
            }
        }

        /// <summary>
        /// Resets the combat moves to initial state when no combo is active
        /// </summary>
        void ResetCombatMoves()
        {
            foreach (var combatMove in CombatMoves)
            {
                if (CombatEntityPosition.Value != combatMove.RequiredPosition)
                {
                    combatMove.SkillTransitionToStateDisable();
                }
                else if(combatMove.NeedActiveTarget
                    &&
                    (OwnerCombatEntity.Value == null || OwnerCombatEntity.Value.Target.Value == null || !OwnerCombatEntity.Value.TargetIsInRange.Value))
                {
                    combatMove.SkillTransitionToStateDisable();
                }
                else if (combatMove.CombatMoveType == CombatMoveType.SingleHit || combatMove.CombatMoveType == CombatMoveType.ComboStarter)
                {
                    combatMove.SkillTransitionToStateEnable();
                }
                else
                {
                    combatMove.SkillTransitionToStateDisable();
                }
            }
        }

        /// <summary>
        /// Disable all other combat moves. Call this when activating any combat move to prevent executing several moves at once.
        /// </summary>
        /// <param name="combatMove">The combat move.</param>
        private void DisableOtherCombatMoves(CombatMove combatMove)
        {
            var movesToDisable = CombatMoves.Where(x => x != combatMove).ToList();
            foreach (var move in movesToDisable)
            {
                move.SkillTransitionToStateDisable();
            }
        }

        #endregion
        #region public functions
        /// <summary>
        /// Enables this combat move set and all of its combat moves.
        /// </summary>
        public void Enable()
        {
            Enabled.Value = true;
            UpdateCombatMoves();
        }

        /// <summary>
        /// Disables this combat move set and all of its combat moves.
        /// </summary>
        public void Disable()
        {
            Enabled.Value = false;
            foreach (var combatMove in CombatMoves)
            {
                combatMove.SkillTransitionToStateDisable();
            }
        }

        /// <summary>
        /// Timeouts the current combo chain and notifies any subscribers.
        /// </summary>
        public void ComboTimeout()
        {
            CombosExecuted.Value = 0;
            UpdateCombatMoves();
            if (ComboTimeoutAction != null)
            {
                ComboTimeoutAction();    
            }
            
        }
        #endregion
    }
}
