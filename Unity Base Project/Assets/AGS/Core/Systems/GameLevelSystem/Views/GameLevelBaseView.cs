using System;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CameraSystem;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.CheckpointSystem;
using AGS.Core.Systems.MissionSystem;
using AGS.Core.Systems.PickUpSystem;
using AGS.Core.Systems.RagdollSystem;
using UnityEngine;

namespace AGS.Core.Systems.GameLevelSystem
{
    /// <summary>
    /// The GameLevel view is an abstract class. Inherit from this to create a game specific level view. Instance views are naturally positioned on an immediate child of the GameManager.
    /// </summary>
    [Serializable]
    public abstract class GameLevelBaseView : ActionView
    {
        #region Public properties
        // References to be set in the editor
        public Transform CheckPointsContainer;
        public Transform EnemiesContainer;
        public Transform PickUpItemsContainer;
        public Transform RagdollsContainer;
        public Transform GameLevelBoundsContainer;
        public CheckPointBaseView ActiveCheckPointView;
        public MissionView MissionView;
        public CameraTargetView CameraTargetView;
        public float StartDelaySeconds;
        #endregion

        public GameLevel GameLevel;
        #region AGS Setup
        public override void SolveModelDependencies(ActionModel model)
        {
            base.SolveModelDependencies(model);
            GameLevel.ActiveCheckPoint.Value = ActiveCheckPointView != null ? ActiveCheckPointView.CheckPoint : null;
            GameLevel.Mission.Value = MissionView != null ? MissionView.Mission : null;
            GameLevel.CameraTarget.Value = CameraTargetView != null ? CameraTargetView.CameraTarget : null;
            if (PickUpItemsContainer != null)
            {
                foreach (var pickUpItemView in PickUpItemsContainer.GetComponentsInChildren<PickUpItemBaseView>())
                {
                    GameLevel.PickUps.Add(pickUpItemView.PickUpItemBase);
                }
            }
            if (CheckPointsContainer != null)
            {
                foreach (var checkPointView in CheckPointsContainer.GetComponentsInChildren<CheckPointBaseView>())
                {
                    GameLevel.CheckPoints.Add(checkPointView.CheckPoint);
                }
            }
            if (EnemiesContainer != null)
            {
                foreach (var enemyBaseView in EnemiesContainer.GetComponentsInChildren<EnemyBaseView>())
                {
                    GameLevel.Enemies.Add(enemyBaseView.Enemy);
                }
            }
            if (GameLevelBoundsContainer != null)
            {
                foreach (var gameLevelBoundView in GameLevelBoundsContainer.GetComponentsInChildren<GameLevelBoundView>())
                {
                    GameLevel.GameLevelBounds.Add(gameLevelBoundView.GameLevelBound);
                }
            }
        }

        public override void InitializeActionModel(ActionModel model)
        {
            base.InitializeActionModel(model);
            GameLevel.Ragdolls.ListItemAdded += RagdollAdded;
        }
        #endregion

        #region MonoBehaviours

        public override void Update()
        {
            base.Update();
            // If there are level bounds, we need to consistently check that characters are inside
            if (GameLevel.GameLevelBounds == null || GameLevel.GameLevelBounds.Count == 0) return;
            if (GameLevel.Player.Value != null)
            {
                GameLevel.CheckCharacterWithinGameLevelBounds(GameLevel.Player.Value);
            }
            for (int index = 0; index < GameLevel.Enemies.Count; index++)
            {
                var enemy = GameLevel.Enemies[index];
                GameLevel.CheckCharacterWithinGameLevelBounds(enemy);
            }
        }

        #endregion

        #region private functions
        /// <summary>
        /// ListItem notification. Ragdoll was added, so we can set up a listner for destroying the view when model is destroyed.
        /// </summary>
        /// <param name="ragdoll">The ragdoll.</param>
        private void RagdollAdded(Ragdoll ragdoll)
        {
            var ragdollObj = Instantiate(Resources.Load(string.Format("Ragdolls/{0}", ragdoll.PrefabName))) as GameObject;
            if (ragdollObj == null) return;
            var ragdollView = ragdollObj.GetComponent<RagdollView>();
            if (ragdollView != null)
            {
                ragdollView.Ragdoll = ragdoll;
                ragdollView.transform.SetParent(RagdollsContainer.transform);
                ragdollView.SetTransform();
                ragdollView.SetMotion();
                ragdoll.ModelDestroyed += () => Destroy(ragdollView.gameObject);

            }

        }
        #endregion private
    }
}