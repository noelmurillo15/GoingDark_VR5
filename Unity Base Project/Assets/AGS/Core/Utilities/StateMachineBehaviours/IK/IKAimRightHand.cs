using UnityEngine;
using System.Collections;
using AGS.Core.Systems.CharacterSystem;

namespace AGS.Core.Utilities.StateMachineBehaviours
{
    public class IKAimRightHand : StateMachineBehaviour
    {
        private CombatEntityBase _combatEntity;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _combatEntity = animator.GetComponentInParent<CombatEntityBaseView>().CombatEntity;
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
            //// Cache the current value of the AimWeight curve.
            //float aimWeight = animator.GetFloat(hash.aimWeightFloat);

            // Set the IK position of the right hand to the player's centre.
            if (_combatEntity.Target.Value != null
                &&
                _combatEntity.Target.Value.Transform != null)
            {
                animator.SetIKPosition(AvatarIKGoal.RightHand, _combatEntity.Target.Value.Transform.position + Vector3.up * _combatEntity.Target.Value.Transform.GetComponent<Collider>().bounds.extents.y);
            }
            else if(_combatEntity.Aiming.Value != null)
            {
                animator.SetIKPosition(AvatarIKGoal.RightHand, _combatEntity.Aiming.Value.AimTarget.Value);
            }

            // Set the weight of the IK compared to animation to that of the curve.
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        }
    }
}