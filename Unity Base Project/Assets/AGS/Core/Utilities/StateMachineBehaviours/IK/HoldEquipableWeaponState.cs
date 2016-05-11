using AGS.Core.Systems.CharacterSystem;
using UnityEngine;

namespace AGS.Core.Utilities.StateMachineBehaviours
{
    /// <summary>
    /// For use with IK weapon handling
    /// </summary>
    public class HoldEquipableWeaponState : StateMachineBehaviour
    {
        public bool SetRotation = true;
        protected CombatEntityBaseView CombatEntityBaseView;
        protected CombatEntityBase CombatEntity;
        private Transform _leftHandTarget;
        private Transform _rightHandTarget;
        
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            CombatEntityBaseView = animator.GetComponentInParent<CombatEntityBaseView>();
            CombatEntity = CombatEntityBaseView.CombatEntity;
            _leftHandTarget = CombatEntity.CurrentWeapon.Value.WeaponGripLeftHand;
            _rightHandTarget = CombatEntity.CurrentWeapon.Value.WeaponGripRightHand;
            
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
        override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Set the right hand target position and rotation, if one has been assigned
            if (_rightHandTarget != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKPosition(AvatarIKGoal.RightHand, _rightHandTarget.position);
                if (SetRotation)
                {
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKRotation(AvatarIKGoal.RightHand, _rightHandTarget.rotation);
                }
            }
            // Set the left hand target position and rotation, if one has been assigned
            if (_leftHandTarget != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, _leftHandTarget.position);
                if (SetRotation)
                {
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                    animator.SetIKRotation(AvatarIKGoal.LeftHand, _leftHandTarget.rotation);
                }
            }
        }
    }
}
