using UnityEngine;

namespace AGS.Core.Utilities.StateMachineBehaviours
{
    public class GrabIKGrips : StateMachineBehaviour
    {
        private Transform _leftHandTarget;
        private Transform _rightHandTarget;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            var rightGrip = GameObject.Find("IKGripRight");
            if (rightGrip)
            {
                _rightHandTarget = rightGrip.transform;
            }
            var leftGrip = GameObject.Find("IKGripLeft");
            if (leftGrip)
            {
                _leftHandTarget = leftGrip.transform;
            }
        
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateIK(animator, stateInfo, layerIndex);


            // Set the left hand target position and rotation, if one has been assigned
            if (_leftHandTarget != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, _leftHandTarget.position);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, _leftHandTarget.rotation);
            }
            // Set the right hand target position and rotation, if one has been assigned
            if (_rightHandTarget != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKPosition(AvatarIKGoal.RightHand, _rightHandTarget.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, _rightHandTarget.rotation);
            }
        }
    }
}
