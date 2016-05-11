using System;
using AGS.Core.Classes.CollisionReferences;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.RagdollSystem;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Systems.InteractionSystem.Interactables;
using UnityEngine;

namespace AGS.Core.Systems.StatusEffectSystem
{
    /// <summary>
    /// AreaOfEffectView needs a collider of some sort set to trigger, to be able to determine its current victims
    /// </summary>
    [Serializable]
    public class AreaOfEffectView : ActionView
    {
        public AreaOfEffect AreaOfEffect;

        #region AGS Setup
        public override void InitializeView()
        {
            AreaOfEffect = new AreaOfEffect();
            SolveModelDependencies(AreaOfEffect);
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);

            // Sets up trigger action enter with KillableBaseView
            Action<KillableBaseView> addKillableTargetAction = AddKillableTarget;
            gameObject.OnCollisionActionEnterWith(addKillableTargetAction);
            gameObject.OnTriggerActionEnterWith(addKillableTargetAction);

            // Sets up trigger action exit with KillableBaseView
            Action<KillableBaseView> removeKillableTargetAction = RemoveKillableTarget;
            gameObject.OnCollisionActionExitWith(removeKillableTargetAction);
            gameObject.OnTriggerActionExitWith(removeKillableTargetAction);

            // Sets up trigger action enter with RagdollBodyTrigger
            Action<RagdollBodyTrigger> addRagdollTargetAction = AddRagdollTarget;
            gameObject.OnCollisionActionEnterWith(addRagdollTargetAction);
            gameObject.OnTriggerActionEnterWith(addRagdollTargetAction);

            // Sets up trigger action exit with RagdollBodyTrigger
            Action<RagdollBodyTrigger> removeRagdollTargetAction = RemoveRagdollTarget;
            gameObject.OnCollisionActionExitWith(removeRagdollTargetAction);
            gameObject.OnTriggerActionExitWith(removeRagdollTargetAction);

            // Sets up trigger action enter with MovableObjectBaseView
            Action<MovableObjectBaseView> addMovableTargetAction = AddMovableTarget;
            gameObject.OnCollisionActionEnterWith(addMovableTargetAction);
            gameObject.OnTriggerActionEnterWith(addMovableTargetAction);


            // Sets up trigger action exit with MovableObjectBaseView
            Action<MovableObjectBaseView> removeMovableTargetAction = RemoveMovableTarget;
            gameObject.OnCollisionActionExitWith(removeMovableTargetAction);
            gameObject.OnTriggerActionExitWith(removeMovableTargetAction);

        }
        #endregion

        #region private functions
        /// <summary>
        /// Adds the killable target.
        /// </summary>
        /// <param name="killableView">The killable view.</param>
        private void AddKillableTarget(KillableBaseView killableView)
        {
            if (killableView.Killable == null) return;
            if (AreaOfEffect.KillableTargets.Contains(killableView.Killable)) return;
            AreaOfEffect.KillableTargets.Add(killableView.Killable);
        }

        /// <summary>
        /// Removes the killable target.
        /// </summary>
        /// <param name="killableView">The killable view.</param>
        private void RemoveKillableTarget(KillableBaseView killableView)
        {
            if (killableView.Killable == null) return;
            if (!AreaOfEffect.KillableTargets.Contains(killableView.Killable)) return;
            AreaOfEffect.KillableTargets.Remove(killableView.Killable);
        }

        /// <summary>
        /// Adds the ragdoll target.
        /// </summary>
        /// <param name="ragdollBody">The ragdoll body.</param>
        private void AddRagdollTarget(MonoBehaviour ragdollBody)
        {
            var ragdollView = ragdollBody.GetComponentInParent<RagdollView>();
            if (ragdollView.Ragdoll == null) return;
            if (AreaOfEffect.RagdollTargets.Contains(ragdollView.Ragdoll)) return;
            AreaOfEffect.RagdollTargets.Add(ragdollView.Ragdoll);

        }

        /// <summary>
        /// Removes the ragdoll target.
        /// </summary>
        /// <param name="ragdollBody">The ragdoll body.</param>
        private void RemoveRagdollTarget(MonoBehaviour ragdollBody)
        {
            var ragdollView = ragdollBody.GetComponentInParent<RagdollView>();
            if (ragdollView.Ragdoll == null) return;
            if (!AreaOfEffect.RagdollTargets.Contains(ragdollView.Ragdoll)) return;
            AreaOfEffect.RagdollTargets.Remove(ragdollView.Ragdoll);

        }

        /// <summary>
        /// Adds the movable target.
        /// </summary>
        /// <param name="movable">The movable.</param>
        private void AddMovableTarget(MovableObjectBaseView movable)
        {
            var movableObjectBaseView = movable.GetComponent<MovableObjectBaseView>();
            if (movableObjectBaseView.MovableObject == null) return;
            if (AreaOfEffect.MovableTargets.Contains(movableObjectBaseView.MovableObject)) return;
            AreaOfEffect.MovableTargets.Add(movableObjectBaseView.MovableObject);

        }

        /// <summary>
        /// Removes the movable target.
        /// </summary>
        /// <param name="movable">The movable.</param>
        private void RemoveMovableTarget(MovableObjectBaseView movable)
        {
            var movableObjectBaseView = movable.GetComponent<MovableObjectBaseView>();
            if (movableObjectBaseView.MovableObject == null) return;
            if (!AreaOfEffect.MovableTargets.Contains(movableObjectBaseView.MovableObject)) return;
            AreaOfEffect.MovableTargets.Remove(movableObjectBaseView.MovableObject);

        }
        #endregion
    }
}