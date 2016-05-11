using AGS.Core.Classes.AvatarReferences;
using AGS.Core.Systems.CharacterSystem;
using UnityEngine;

namespace AGS.Core.Utilities.StateMachineBehaviours
{
    /// <summary>
    /// For grabbing interaction targets
    /// </summary>
    public class GrabInteractionTargets : StateMachineBehaviour
    {
        public bool GrabSmoothly;
        private AdvancedCharacterBase _advancedCharacter;
        private float _currentPosWeight;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _advancedCharacter = animator.GetComponentInParent<AdvancedCharacterBaseView>().AdvancedCharacter;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            _currentPosWeight = 0f;
        }

        // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
        override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_advancedCharacter == null
                ||
                _advancedCharacter.InteractionSkills.Value == null
                ||
                _advancedCharacter.InteractionSkills.Value.CurrentInteractableTarget.Value == null)
            {
                return;
            }
            var leftHandTarget = _advancedCharacter.InteractionSkills.Value.CurrentInteractableTarget.Value.Transform.GetComponentInChildren<IKTargetLeftHand>();
            var rightHandTarget = _advancedCharacter.InteractionSkills.Value.CurrentInteractableTarget.Value.Transform.GetComponentInChildren<IKTargetRightHand>();

            if (GrabSmoothly && _currentPosWeight < 0.995f)
            {
                _currentPosWeight = Mathf.Lerp(_currentPosWeight, 1f, 0.1f);
            }
            else
            {
                _currentPosWeight = 1f;
            }
            if (leftHandTarget)
            {
                Transform leftHandTargetTransform = leftHandTarget.transform;
                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTargetTransform.position);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTargetTransform.localRotation);                
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, _currentPosWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, _currentPosWeight);


            }
            if (rightHandTarget)
            {
                Transform rightHandTargetTransform = rightHandTarget.transform;
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTargetTransform.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTargetTransform.localRotation);
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, _currentPosWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, _currentPosWeight);
            }
        }
    }
}
