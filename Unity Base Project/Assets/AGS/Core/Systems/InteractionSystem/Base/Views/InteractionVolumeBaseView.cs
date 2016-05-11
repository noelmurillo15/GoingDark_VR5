using System;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterControlSystem;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.InteractionSystem.Interactables;
using UnityEngine;

namespace AGS.Core.Systems.InteractionSystem.Base
{
    /// <summary>
    /// InteractionVolumeViews are set up to trigger with InteractableBaseViews throughout the game level
    /// </summary>
    [Serializable]
    public abstract class InteractionVolumeBaseView : ActionView
    {
        #region Public properties
        // Field to be set in the editor
        public InteractionTargetHeight TargetHeight;
        #endregion

        public InteractionVolume InteractionVolume;


        #region convenience properties
        /// <summary>
        /// Gets the owner interaction skills.
        /// </summary>
        /// <value>
        /// The owner interaction skills.
        /// </value>
        protected InteractionSkills OwnerInteractionSkills
        {
            get { return InteractionVolume.OwnerInteractionSkills.Value; }
        }

        protected AdvancedCharacterBase OwnerCharacter
        {
            get { return InteractionVolume.OwnerInteractionSkills.Value.OwnerCharacter.Value; }
        }
        /// <summary>
        /// Gets the character transform.
        /// </summary>
        /// <value>
        /// The character transform.
        /// </value>
        protected Transform CharTransform
        {
            get { return InteractionVolume.OwnerInteractionSkills.Value.OwnerCharacter.Value.Transform; }
        }

        /// <summary>
        /// Gets the owner character controller.
        /// </summary>
        /// <value>
        /// The owner character controller.
        /// </value>
        protected CharacterControllerBase OwnerCharacterController
        {
            get { return InteractionVolume.OwnerInteractionSkills.Value.OwnerCharacter.Value.CharacterController.Value; }
        }

        /// <summary>
        /// Gets or sets the current interactable target.
        /// </summary>
        /// <value>
        /// The current interactable target.
        /// </value>
        protected InteractableBase CurrentInteractableTarget
        {
            get { return InteractionVolume.OwnerInteractionSkills.Value.CurrentInteractableTarget.Value; }
            set { InteractionVolume.OwnerInteractionSkills.Value.CurrentInteractableTarget.Value = value; }
        }

        /// <summary>
        /// Gets the current interaction volume.
        /// </summary>
        /// <value>
        /// The current interaction volume.
        /// </value>
        protected InteractionVolume CurrentInteractionVolume
        {
            get { return InteractionVolume.OwnerInteractionSkills.Value.CurrentInteractionVolume.Value; }
        }
        #endregion

        #region AGS Setup
        public override void InitializeView()
        {
            InteractionVolume = new InteractionVolume(TargetHeight);
            SolveModelDependencies(InteractionVolume);
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);

            // Set up Trigger subscriptions with MovableObjectBaseView
            Action<MovableObjectBaseView> movableObjectEnterAction = OnMovableObjectEnter;
            gameObject.OnTriggerActionEnterWith(movableObjectEnterAction);
            Action<MovableObjectBaseView> movableObjectStayAction = OnMovableObjectStay;
            gameObject.OnTriggerActionStayWith(movableObjectStayAction);
            Action<MovableObjectBaseView> movableObjectExitAction = OnMovableObjectExit;
            gameObject.OnTriggerActionExitWith(movableObjectExitAction);

            // Set up Trigger subscriptions with LadderView
            Action<LadderView> ladderEnterAction = OnLadderEnter;
            gameObject.OnTriggerActionEnterWith(ladderEnterAction);
            Action<LadderView> ladderStayAction = OnLadderStay;
            gameObject.OnTriggerActionStayWith(ladderStayAction);
            Action<LadderView> ladderExitAction = OnLadderExit;
            gameObject.OnTriggerActionExitWith(ladderExitAction);

            // Set up Trigger subscriptions with LedgeView
            Action<LedgeView> ledgeEnterAction = OnLedgeEnter;
            gameObject.OnTriggerActionEnterWith(ledgeEnterAction);
            Action<LedgeView> ledgeStayAction = OnLedgeStay;
            gameObject.OnTriggerActionStayWith(ledgeStayAction);
            Action<LedgeView> ledgeExitAction = OnLedgeExit;
            gameObject.OnTriggerActionExitWith(ledgeExitAction);

            // Set up Trigger subscriptions with SwingUnitBaseView
            Action<SwingUnitBaseView> swingUnitEnterAction = OnSwingUnitEnter;
            gameObject.OnTriggerActionEnterWith(swingUnitEnterAction);
            Action<SwingUnitBaseView> swingUnitStayAction = OnSwingUnitStay;
            gameObject.OnTriggerActionStayWith(swingUnitStayAction);
            Action<SwingUnitBaseView> swingUnitExitAction = OnSwingUnitExit;
            gameObject.OnTriggerActionExitWith(swingUnitExitAction);

            // Set up Trigger subscriptions with SwitchView
            Action<SwitchView> switchEnterAction = OnSwitchEnter;
            gameObject.OnTriggerActionEnterWith(switchEnterAction);
            Action<SwitchView> switchStayAction = OnSwitchStay;
            gameObject.OnTriggerActionStayWith(switchStayAction);
            Action<SwitchView> switchExitAction = OnSwitchExit;
            gameObject.OnTriggerActionExitWith(switchExitAction);
        }
        #endregion

        #region interaction functions
        #region movable objects
        /// <summary>
        /// Called when [movable object enter].
        /// </summary>
        /// <param name="movableObjectBaseView">The movable object base view.</param>
        protected virtual void OnMovableObjectEnter(MovableObjectBaseView movableObjectBaseView) { }

        /// <summary>
        /// Called when [movable object stay].
        /// </summary>
        /// <param name="movableObjectBaseView">The movable object base view.</param>
        protected virtual void OnMovableObjectStay(MovableObjectBaseView movableObjectBaseView){}

        /// <summary>
        /// Called when [movable object exit].
        /// </summary>
        /// <param name="movableObjectBaseView">The movable object base view.</param>
        protected virtual void OnMovableObjectExit(MovableObjectBaseView movableObjectBaseView){}
        #endregion

        #region ladders
        /// <summary>
        /// Called when [ladder enter].
        /// </summary>
        /// <param name="ladderView">The ladder view.</param>
        protected virtual void OnLadderEnter(LadderView ladderView) { }

        /// <summary>
        /// Called when [ladder stay].
        /// </summary>
        /// <param name="ladderView">The ladder view.</param>
        protected virtual void OnLadderStay(LadderView ladderView){}

        /// <summary>
        /// Called when [ladder exit].
        /// </summary>
        /// <param name="ladderView">The ladder view.</param>
        protected virtual void OnLadderExit(LadderView ladderView){}
        #endregion

        #region ledges
        /// <summary>
        /// Called when [ledge enter].
        /// </summary>
        /// <param name="ledge">The ledge.</param>
        protected virtual void OnLedgeEnter(LedgeView ledge) { }

        /// <summary>
        /// Called when [ledge stay].
        /// </summary>
        /// <param name="ledge">The ledge.</param>
        protected virtual void OnLedgeStay(LedgeView ledge){}

        /// <summary>
        /// Called when [ledge exit].
        /// </summary>
        /// <param name="ledge">The ledge.</param>
        protected virtual void OnLedgeExit(LedgeView ledge) { }
        #endregion

        #region swings
        /// <summary>
        /// Called when [swing unit enter].
        /// </summary>
        /// <param name="swingUnitBaseView">The swing unit base view.</param>
        protected virtual void OnSwingUnitEnter(SwingUnitBaseView swingUnitBaseView) { }

        /// <summary>
        /// Called when [swing unit stay].
        /// </summary>
        /// <param name="swingUnitBaseView">The swing unit base view.</param>
        protected virtual void OnSwingUnitStay(SwingUnitBaseView swingUnitBaseView) { }

        /// <summary>
        /// Called when [swing unit exit].
        /// </summary>
        /// <param name="swingUnitBaseView">The swing unit base view.</param>
        protected virtual void OnSwingUnitExit(SwingUnitBaseView swingUnitBaseView) { }
        #endregion

        #region switches
        /// <summary>
        /// Called when [switch enter].
        /// </summary>
        /// <param name="switchView">The switch view.</param>
        protected virtual void OnSwitchEnter(SwitchView switchView) { }

        /// <summary>
        /// Called when [switch stay].
        /// </summary>
        /// <param name="switchView">The switch view.</param>
        protected virtual void OnSwitchStay(SwitchView switchView) { }

        /// <summary>
        /// Called when [switch exit].
        /// </summary>
        /// <param name="switchView">The switch view.</param>
        protected virtual void OnSwitchExit(SwitchView switchView) { }
        #endregion
        #endregion
    }
}