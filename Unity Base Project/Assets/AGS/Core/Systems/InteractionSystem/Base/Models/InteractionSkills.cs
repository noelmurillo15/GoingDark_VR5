using AGS.Core.Classes.ActionProperties;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.InteractionSystem.Interactables;
using AGS.Core.Systems.InteractionSystem.InteractionSkills.LadderClimbing;
using AGS.Core.Systems.InteractionSystem.InteractionSkills.LedgeClimbing;
using AGS.Core.Systems.InteractionSystem.InteractionSkills.ObjectMovement;
using AGS.Core.Systems.InteractionSystem.InteractionSkills.Swinging;
using AGS.Core.Systems.InteractionSystem.InteractionSkills.SwitchInteraction;

namespace AGS.Core.Systems.InteractionSystem.Base
{
    /// <summary>
    /// A bundle of all available interaction skills, plus a set of interaction volumes that can be used for identifying interaction views throughout the game level.
    /// </summary>
    public class InteractionSkills : ActionModel
    {
        #region Properties
        // Subscribable properties
        public ActionProperty<AdvancedCharacterBase> OwnerCharacter; // Reference to owner character
        public ActionProperty<LadderClimbing> LadderClimbing { get; set; }
        public ActionProperty<LedgeClimbing> LedgeClimbing { get; set; }
        public ActionProperty<ObjectMovement> ObjectMovement { get; set; }
        public ActionProperty<Swinging> Swinging { get; set; }
        public ActionProperty<SwitchInteraction> SwitchInteraction { get; set; }

        public ActionProperty<InteractableBase> CurrentInteractableTarget { get; set; } // A reference to the InteractionObject we are currently interacting with.
        public ActionProperty<InteractionVolume> CurrentInteractionVolume { get; set; } // A reference to the InteractionVolume that was used to initilize the current interaction.

        public ActionList<InteractionVolume> InteractionVolumes { get; private set; }
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="InteractionSkills"/> class.
        /// </summary>
        public InteractionSkills()
        {
            OwnerCharacter = new ActionProperty<AdvancedCharacterBase>();
            LadderClimbing = new ActionProperty<LadderClimbing>();
            LadderClimbing.OnValueChanged += (sender, interactionSkill) => SetParent(interactionSkill.Value);
            LedgeClimbing = new ActionProperty<LedgeClimbing>();
            LedgeClimbing.OnValueChanged += (sender, interactionSkill) => SetParent(interactionSkill.Value);
            ObjectMovement = new ActionProperty<ObjectMovement>();
            ObjectMovement.OnValueChanged += (sender, interactionSkill) => SetParent(interactionSkill.Value);
            Swinging = new ActionProperty<Swinging>();
            Swinging.OnValueChanged += (sender, interactionSkill) => SetParent(interactionSkill.Value);
            SwitchInteraction = new ActionProperty<SwitchInteraction>();
            SwitchInteraction.OnValueChanged += (sender, interactionSkill) => SetParent(interactionSkill.Value);

            CurrentInteractableTarget = new ActionProperty<InteractableBase>();
            CurrentInteractableTarget.OnValueChanged += (sender, currentTarget) =>
            {
                var swingUnit = currentTarget.Value as SwingUnit;
                if (swingUnit != null)
                {
                    // If we are currently interacting with a SwingUnit, check its placement
                    CheckSwingUnit(swingUnit);
                }
            };
            CurrentInteractionVolume = new ActionProperty<InteractionVolume>();
            InteractionVolumes = new ActionList<InteractionVolume>();
            InteractionVolumes.ListItemAdded += InteractionVolumeAdded;

        }



        #region private functions
        /// <summary>
        /// Sets the parent OwnerInteractionSkills for an interaction skill.
        /// </summary>
        /// <param name="interactionSkill">The interaction skill.</param>
        private void SetParent(InteractionSkillBase interactionSkill)
        {
            interactionSkill.OwnerInteractionSkills.Value = this;
        }

        /// <summary>
        /// ListItem notificaiton. InteractionVolume was added
        /// </summary>
        /// <param name="interactionVolumeAdd">The interaction volume add.</param>
        private void InteractionVolumeAdded(InteractionVolume interactionVolumeAdd)
        {
            interactionVolumeAdd.OwnerInteractionSkills.Value = this;
        }

        /// <summary>
        /// Checks the swing unit.
        /// Notification that we are interacting with a swing unit. Check if we are on top or bottom of swing
        /// </summary>
        /// <param name="currentSwingUnit">The current swing unit.</param>
        private void CheckSwingUnit(SwingUnit currentSwingUnit)
        {
            Swinging.Value.OnTopOfSwing.Value = currentSwingUnit.IsBaseUnit;
            Swinging.Value.OnEndOfSwing.Value = currentSwingUnit.IsEndUnit;
        }
        #endregion
        #region public functions
        /// <summary>
        /// Clears the interaction volume.
        /// </summary>
        /// <param name="interactionVolume">The interaction volume.</param>
        public void ClearInteractionVolume(InteractionVolume interactionVolume)
        {
            // only clear if interactionVolume parameter is the CurrentInteractionVolume
            if (CurrentInteractionVolume.Value == interactionVolume)
            {
                CurrentInteractionVolume.Value = null;
            }
        }

        /// <summary>
        /// Forcing a clear of both CurrentInteractableTarget and CurrentInteractionVolume.
        /// </summary>
        public void ForceClear()
        {
            CurrentInteractableTarget.Value = null;
            CurrentInteractionVolume.Value = null;
        }
        #endregion
    }
}
