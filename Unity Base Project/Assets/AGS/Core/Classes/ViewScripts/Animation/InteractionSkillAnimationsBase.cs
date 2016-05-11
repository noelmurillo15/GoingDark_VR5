using AGS.Core.Enums;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.InteractionSystem.Interactables;
using UnityEngine;

namespace AGS.Core.Classes.ViewScripts
{
    /// <summary>
    /// ViewScript that subscribes to InteractionSkills and sets animator parameters 
    /// </summary>
    public abstract class InteractionSkillAnimationsBase : ViewScriptBase
    {
        protected Animator Animator;

        protected AdvancedCharacterBaseView AdvancedCharacterBaseView;
        protected AdvancedCharacterBase AdvancedCharacter;

        public override void Awake()
        {
            base.Awake();
            Animator = gameObject.GetComponentInChildren<Animator>();

        }

        protected override void SetupModelBindings()
        {
            if (ViewReference != null)
            {
                AdvancedCharacterBaseView = ViewReference as AdvancedCharacterBaseView;
                if (AdvancedCharacterBaseView == null) return;

                AdvancedCharacter = AdvancedCharacterBaseView.AdvancedCharacter;

            }
            if (AdvancedCharacter == null) return;

            if (AdvancedCharacter.InteractionSkills.Value == null) return;
            if (AdvancedCharacter.InteractionSkills.Value.LedgeClimbing.Value != null)
            {
                AdvancedCharacter.InteractionSkills.Value.LedgeClimbing.Value.LedgeClimbingCurrentState.OnValueChanged += (sender, state) => OnLedgeClimbingStateChanged(state.Value);
            }
            if (AdvancedCharacter.InteractionSkills.Value.Swinging.Value != null)
            {
                AdvancedCharacter.InteractionSkills.Value.Swinging.Value.SwingingCurrentState.OnValueChanged += (sender, state) => OnSwingingStateChanged(state.Value);
            }
            if (AdvancedCharacter.InteractionSkills.Value.LadderClimbing.Value != null)
            {
                AdvancedCharacter.InteractionSkills.Value.LadderClimbing.Value.LadderClimbingCurrentState.OnValueChanged += (sender, state) => OnLadderClimbingStateChanged(state.Value);
            }
            if (AdvancedCharacter.InteractionSkills.Value.ObjectMovement.Value != null)
            {
                AdvancedCharacter.InteractionSkills.Value.ObjectMovement.Value.ObjectMovementCurrentState.OnValueChanged += (sender, state) => OnObjectMovementStateChanged(state.Value);
            }
            if (AdvancedCharacter.InteractionSkills.Value.SwitchInteraction.Value != null)
            {
                AdvancedCharacter.InteractionSkills.Value.SwitchInteraction.Value.SwitchInteractionCurrentState.OnValueChanged += (sender, state) => OnSwitchInteractionStateChanged(state.Value);
            }
        }

        /// <summary>
        /// Called when [object movement state changed].
        /// </summary>
        /// <param name="state">The state.</param>
        protected abstract void OnObjectMovementStateChanged(ObjectMovementState state);

        /// <summary>
        /// Called when [ledge climbing state changed].
        /// </summary>
        /// <param name="state">The state.</param>
        protected abstract void OnLedgeClimbingStateChanged(LedgeClimbingState state);

        /// <summary>
        /// Called when [swinging state changed].
        /// </summary>
        /// <param name="state">The state.</param>
        protected abstract void OnSwingingStateChanged(SwingingState state);

        /// <summary>
        /// Called when [ladder climbing state changed].
        /// </summary>
        /// <param name="state">The state.</param>
        protected abstract void OnLadderClimbingStateChanged(LadderClimbingState state);

        /// <summary>
        /// Called when [switch interaction state changed].
        /// </summary>
        /// <param name="state">The state.</param>
        protected abstract void OnSwitchInteractionStateChanged(SwitchInteractionState state);
    }
}
