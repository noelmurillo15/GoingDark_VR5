using System;
using AGS.Core.Classes.AvatarReferences;
using AGS.Core.Classes.DataClasses;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterControlSystem;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.SkillSystem;
using AGS.Core.Systems.WeaponSystem;
using UnityEngine;

namespace AGS.Core.Systems.CombatSkillSystem
{
    /// <summary>
    /// ThrowingSkillView
    /// </summary>
    [Serializable]
    public class ThrowingSkillView : SkillBaseView
    {

        #region Public properties
        // Fields to be set in the editor
        public ThrowingSkillHand ThrowingSkillHand;
        public Transform ThrowablesContainer; // GameLevels throwables container
        public ThrowableWeaponTypeSkillData[] ThrowableWeaponTypeData; // set the data of different throwable weapons in the editor
        #endregion

        public ThrowingSkill ThrowingSkill;

        // Short hand props
        /// <summary>
        /// Gets the owner character.
        /// </summary>
        /// <value>
        /// The owner character.
        /// </value>
        protected CharacterBase OwnerCharacter
        {
            get { return ThrowingSkill.OwnerCharacter.Value; }
        }
        /// <summary>
        /// Gets the owner character controller.
        /// </summary>
        /// <value>
        /// The owner character controller.
        /// </value>
        protected CharacterControllerBase OwnerCharacterController
        {
            get { return ThrowingSkill.OwnerCharacter.Value.CharacterController.Value; }
        }

        private UpdatePersistantGameObject _throwingCheckThrowUpdate;

        #region AGS Setup
        public override void InitializeView()
        {
            switch (ThrowingSkillHand)
            {
                case ThrowingSkillHand.Left:
                    ThrowingSkill = new ThrowingSkill(transform.parent.GetComponentInChildren<AvatarHandL>().transform, ThrowableWeaponTypeData);
                    break;
                case ThrowingSkillHand.Right:
                    ThrowingSkill = new ThrowingSkill(transform.parent.GetComponentInChildren<AvatarHandR>().transform, ThrowableWeaponTypeData);
                    break;
            }

            SolveModelDependencies(ThrowingSkill);
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            ThrowingSkill.IsEnabled.OnValueChanged += (sender, isEnabled) =>
            {
                // Stop updating when skill is disabled
                if (isEnabled.Value)
                {
                    _throwingCheckThrowUpdate = ComponentExtensions.SetupComponent<UpdatePersistantGameObject>(gameObject);
                    _throwingCheckThrowUpdate.UpdateMethod = () =>
                    {
                        if (CheckBeginThrow() && ThrowingSkill.BeginThrowAction != null)
                        {
                            ThrowingSkill.BeginThrowAction();
                        }

                    };
                }
                else if (_throwingCheckThrowUpdate != null)
                {
                    _throwingCheckThrowUpdate.Stop();
                }
            };
            ThrowingSkill.ReleaseThrowableAction += CreateAndTrowTrowable;
        }
        #endregion


        #region MonoBehaviours
        public override void Awake()
        {
            base.Awake();
            if (ThrowablesContainer == null && GameObject.Find("Throwables") != null)
            {
                ThrowablesContainer = GameObject.Find("Throwables").transform;
            }
        }
        #endregion

        #region private functions/
        /// <summary>
        /// Update function. Check for throwing skill input
        /// </summary>
        /// <returns></returns>
        private bool CheckBeginThrow()
        {
            if (OwnerCharacter == null) return false;
            return OwnerCharacterController.Attack3.Value && !ThrowingSkill.OutOfResources.Value && !ThrowingSkill.OutOfThrowables;
        }

        /// <summary>
        /// Instaniate a throwable weapon view, add its model to CharacterThrownThrowables and then throw it.
        /// </summary>
        /// <param name="throwableWeaponType">Type of the throwable weapon.</param>
        /// <param name="throwableWeaponThrowingType">Type of the throwable weapon throwing.</param>
        /// <param name="throwingSpeed">The throwing speed.</param>
        private void CreateAndTrowTrowable(ThrowableWeaponType throwableWeaponType, ThrowableWeaponThrowingType throwableWeaponThrowingType, Vector3 throwingSpeed)
        {
            // For forward throwing type, we can release with z position = 0, but arc throwing releases at the shoulder and need to avoid hitting one owns head
            var releasePosition = throwableWeaponThrowingType == ThrowableWeaponThrowingType.Forward
                ? new Vector3(ThrowingSkill.ThrowingSkillMuzzle.position.x, ThrowingSkill.ThrowingSkillMuzzle.position.y, 0f)
                : ThrowingSkill.ThrowingSkillMuzzle.position;
            var throwableObj = Instantiate(Resources.Load(string.Format("Throwables/{0}", throwableWeaponType))) as GameObject;
            if (throwableObj == null) return;
            throwableObj.transform.position = releasePosition;
            var throwableWeaponView = throwableObj.GetComponent<ThrowableWeaponBaseView>();
            if (throwableWeaponView != null)
            {
                throwableWeaponView.transform.parent = ThrowablesContainer;
                throwableWeaponView.ViewReady.OnValueChanged += (sender, viewReady) =>
                {
                    if (!viewReady.Value) return;
                    OwnerCharacter.ThrownTrowables.Add(throwableWeaponView.ThrowableWeapon);
                    throwableWeaponView.ThrowableWeapon.Throw(throwingSpeed);
                };
            }
        }
        #endregion
    }
}