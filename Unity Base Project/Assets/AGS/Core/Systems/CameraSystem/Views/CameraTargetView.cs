using System;
using AGS.Core.Systems.BaseSystem;
using UnityEngine;

namespace AGS.Core.Systems.CameraSystem
{
    /// <summary>
    /// Attach CameraTargetView to a GameObject that should follow the player, with an optional offset. Then the MainCamera to follow the CameraTargetView
    /// Make sure that both the Player and the top most GameObject in the ragdoll heirarchy with a Rigidbody, has the "Player" tag
    /// _targetObj will automatically find its new target when the CameraTarget.FollowRagdoll value is changed
    /// </summary>
    [Serializable]
    public class CameraTargetView : ActionView
    {
        private const string TargetTag = "Player";
        private GameObject _targetObj;

        #region Public properties
        public Vector3 Offset;
        #endregion

        public CameraTarget CameraTarget;

        #region AGS Setup
        public override void InitializeView()
        {
            CameraTarget = new CameraTarget(Offset);
            SolveModelDependencies(CameraTarget);
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            CameraTarget.FollowRagdoll.OnValueChanged += (sender, followRagdoll) =>
            {
                _targetObj = null;
            };
        }
        #endregion

        #region MonoBehaviours

        public override void Update()
        {
            base.Update();
            if (_targetObj != null)
            {
                transform.position = _targetObj.transform.position + CameraTarget.Offset;
            }
            else
            {
                _targetObj = GameObject.FindWithTag(TargetTag);
            }
        }

        #endregion
    }
}