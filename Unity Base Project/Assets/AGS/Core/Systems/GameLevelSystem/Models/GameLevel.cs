using System;
using System.Linq;
using System.Security.Cryptography;
using AGS.Core.Base;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.MonoExtensions;
using AGS.Core.Classes.TimerComponents;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CameraSystem;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.CheckpointSystem;
using AGS.Core.Systems.MissionSystem;
using AGS.Core.Systems.PickUpSystem;
using AGS.Core.Systems.RagdollSystem;

namespace AGS.Core.Systems.GameLevelSystem
{
    /// <summary>
    /// Main game level model. AGS scene is not required to have a GameLevel, but it is the general case.
    /// GameLevel tracks player activity, handles game states and tracks objects in the level like PickUps, CheckPoints and Enemies.
    /// </summary>
    public class GameLevel : ActionModel
    {
        #region Properties
        public float GameLevelStartDelay { get; private set; } // Delay before starting the level

        // Subscribable properties
        public ActionProperty<Player> Player { get; private set; }
        public ActionProperty<CheckPoint> ActiveCheckPoint { get; private set; } // Current check point player will return to at restart
        public ActionProperty<CameraTarget> CameraTarget { get; private set; }
        public ActionProperty<Mission> Mission { get; private set; } // Reference to the Mission to determine game level status based on mission requirements
        public ActionProperty<GameLevelState> GameLevelCurrentState { get; set; }
        public ActionList<GameLevelBound> GameLevelBounds { get; private set; } // Bounds are used to protect characters from going somewhere unexpected
        public ActionList<PickUpItemBase> PickUps { get; private set; } // PickUps of all pick up types
        public ActionList<PickUpItemBase> PickUpsTemp { get; private set; } // Pick up what are collected but not yet deleted are in stored here. They can be restored if player dies
        public ActionList<Ragdoll> Ragdolls { get; private set; }
        public ActionList<Enemy> Enemies { get; private set; }
        public ActionList<CheckPoint> CheckPoints { get; private set; }

        // Subscribables. Both the GameLevelBaseView and the HUDView are typical subscribers to these Actions.
        public Action PlayerDiedAction { get; set; }
        public Action<bool> PlayerIsDetectedAction { get; set; }
        public Action LevelFinishedAction { get; set; }
        public Action LevelFailedAction { get; set; }
        public Action PlayerDeadAction { get; set; }
        public Action LevelRestartAction { get; set; }
        public Action ItemPickUpAction { get; set; }
        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="GameLevel"/> class.
        /// </summary>
        /// <param name="levelStartDelay">The level start delay.</param>
        public GameLevel(float levelStartDelay)
        {
            GameLevelStartDelay = levelStartDelay;
            Player = new ActionProperty<Player>();
            Player.OnValueChanged += (sender, player) => PlayerChanged(player.Value);
            ActiveCheckPoint = new ActionProperty<CheckPoint>();
            CameraTarget = new ActionProperty<CameraTarget>();
            Mission = new ActionProperty<Mission>();
            Mission.OnValueChanged += (sender, mission) => OnMissionChanged(mission.Value);
            GameLevelCurrentState = new ActionProperty<GameLevelState>();
            GameLevelBounds = new ActionList<GameLevelBound>();
            PickUps = new ActionList<PickUpItemBase>();
            PickUps.ListItemAdded += PickUpItemAdded;
            PickUps.ListItemRemoved += PickUpItemRemoved;
            PickUpsTemp = new ActionList<PickUpItemBase>();
            PickUpsTemp.ListItemRemoved += PickUpTempItemRemoved;
            CheckPoints = new ActionList<CheckPoint>();
            CheckPoints.ListItemAdded += CheckPointAdded;
            Ragdolls = new ActionList<Ragdoll>();
            Enemies = new ActionList<Enemy>();
            Enemies.ListItemAdded += EnemyAdded;
            Enemies.ListItemRemoved += EnemyRemoved;

            if (levelStartDelay > 0f)
            {
                var levelStartTimer = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(GameManager.TemporaryTimerComponents, "Start timer");
                levelStartTimer.TimerMethod = TransitionToStateRun;
                levelStartTimer.Invoke(levelStartDelay); 
            }
            else
            {
                TransitionToStateRun();
            }
        }

        #region private functions
        /// <summary>
        /// Called when [mission changed].
        /// </summary>
        /// <param name="mission">The mission.</param>
        public void OnMissionChanged(Mission mission)
        {
            mission.MissionCurrentState.OnValueChanged += (sender, state) => OnMissionStateChanged(state.Value);
        }

        /// <summary>
        /// Called when [mission state changed].
        /// </summary>
        /// <param name="missionState">State of the mission.</param>
        private void OnMissionStateChanged(MissionState missionState)
        {
            switch (missionState)
            {
                case MissionState.Completed:
                    TransitionToStateComplete();
                    break;
                case MissionState.Failed:
                    TransitionToStateFail();
                    break;
            }
        }
       
        /// <summary>
        /// This function is generally called only when a new player is instantiated.
        /// </summary>
        /// <param name="player">The player.</param>
        private void PlayerChanged(Player player)
        {
            if (player == null) return;
            DestroyPlayerRagdoll();
            if (CameraTarget.Value != null)
            {
                CameraTarget.Value.CameraFollowPlayer();
            }
            player.OwnerGameLevel.Value = this;
            if (ActiveCheckPoint.Value != null)
            {
                player.Transform.position = ActiveCheckPoint.Value.Position;    
            }
            
            player.DamageableCurrentState.OnValueChanged += (sender, state) => OnPlayerStateChanged(state.Value);
        }

        /// <summary>
        /// Destroys the player ragdoll.
        /// </summary>
        private void DestroyPlayerRagdoll()
        {
            var playerRagdoll = UnityEngine.Object.FindObjectOfType<PlayerRagdollView>();
            if (playerRagdoll == null) return;

            playerRagdoll.Ragdoll.DestroyModel();
            Ragdolls.Remove(playerRagdoll.Ragdoll);
        }

        /// <summary>
        /// If there are bounds set up, make sure all characters stay within them.
        /// </summary>
        /// <param name="characterBase">The character base.</param>
        internal void CheckCharacterWithinGameLevelBounds(CharacterBase characterBase)
        {
            if (characterBase == null || characterBase.Transform == null) return;
            foreach (var bound in GameLevelBounds)
            {
                switch (bound.LimitSide)
                {
                    case GameLevelLimitSide.Left:
                        if (characterBase.Transform.position.x <= bound.Position.x)
                        {
                            if (!characterBase.FacingGameLevelForward.Value)
                            {
                                HandleBoundHit(characterBase, bound, true);
                            }
                            else if (characterBase.CharacterOutOfBoundsHorizontal.Value)
                            {
                                characterBase.CharacterOutOfBoundsHorizontal.Value = false;
                            }
                        }
                        break;
                    case GameLevelLimitSide.Right:
                        if (characterBase.Transform.position.x >= bound.Position.x)
                        {
                            if (characterBase.FacingGameLevelForward.Value)
                            {
                                HandleBoundHit(characterBase, bound, true);
                            }
                            else if (characterBase.CharacterOutOfBoundsHorizontal.Value)
                            {
                                characterBase.CharacterOutOfBoundsHorizontal.Value = false;
                            }
                        }
                        break;
                    case GameLevelLimitSide.Top:
                        if (characterBase.Transform.position.y >= bound.Position.y)
                        {
                            HandleBoundHit(characterBase, bound, false);
                        }
                        else
                        {
                            if (characterBase.CharacterOutOfBoundsVertical.Value)
                            {
                                characterBase.CharacterOutOfBoundsVertical.Value = false;
                            }
                        }
                        break;
                    case GameLevelLimitSide.Bottom:
                        if (characterBase.Transform.position.y <= bound.Position.y)
                        {
                            HandleBoundHit(characterBase, bound, false);
                        }
                        else
                        {
                            if (characterBase.CharacterOutOfBoundsVertical.Value)
                            {
                                characterBase.CharacterOutOfBoundsVertical.Value = false;
                            }
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Handles game level bounds hits.
        /// </summary>
        /// <param name="character">The character.</param>
        /// <param name="bound">The GameLevelBound.</param>
        /// <param name="horizontal">Determines if to check horizontal or vertical.</param>
        private void HandleBoundHit(CharacterBase character, GameLevelBound bound, bool horizontal)
        {
            if (horizontal)
            {
                switch (bound.Type)
                {
                    case GameLevelBoundType.Block:
                        if (!character.CharacterOutOfBoundsHorizontal.Value)
                        {
                            character.CharacterOutOfBoundsHorizontal.Value = true;
                        }

                        break;
                    case GameLevelBoundType.Kill:
                        character.InstantDeath();
                        break;
                }
            }
            else
            {
                switch (bound.Type)
                {
                    case GameLevelBoundType.Block:
                        character.CharacterOutOfBoundsVertical.Value = true;
                        break;
                    case GameLevelBoundType.Kill:
                        character.InstantDeath();
                        break;
                }
            }
        }

        /// <summary>
        /// Called when [player state changed].
        /// </summary>
        /// <param name="damageableState">State of the damageable.</param>
        private void OnPlayerStateChanged(DamageableState damageableState)
        {
            if (damageableState == DamageableState.Destroyed)
            {
                // small delay to make sure any forces are applied before playerdied command, which switches to ragdoll
                var timerComponent = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(GameManager.TemporaryTimerComponents);
                timerComponent.TimerMethod = PlayerDied;
                timerComponent.Invoke(0.1f);
                TimerComponents.Add(timerComponent);
            }
        }

        /// <summary>
        /// Call this when player dies. Notifies any subscribers that player died and creates a ragdoll for player.
        /// </summary>
        private void PlayerDied()
        {
            if (PlayerDiedAction != null)
            {
                PlayerDiedAction();
            }

            // Store the player position and motion, then destroy player
            if (Player.Value == null) return;
            var charMotion = Player.Value.CurrentMotion.Value;
            var targetTransform = Player.Value.Transform;
            var playerRagdollName = Player.Value.Name;
            Player.Value.DestroyModel();

            // Create player ragdoll
            var ragdoll = new PlayerRagdoll(string.Format("{0}{1}", playerRagdollName, "Ragdoll"));
            CameraTarget.Value.CameraFollowRagdoll();
            ragdoll.SetTransform(targetTransform);
            ragdoll.SetMotion(charMotion);
            Ragdolls.Add(ragdoll);

            TransitionToStatePlayerDeath();
        }

        /// <summary>
        /// ListItem notification. Checkpoint was added.
        /// </summary>
        /// <param name="checkpoint">The checkpoint.</param>
        private void CheckPointAdded(CheckPoint checkpoint)
        {
            if (ActiveCheckPoint.Value == null && checkpoint.LevelStart)
            {
                ActiveCheckPoint.Value = checkpoint;
            }

            // Get notified of when the player reaches a checkpoint
            checkpoint.CheckPointReachedAction += () =>
            {
                if (ActiveCheckPoint.Value == null
                    ||
                    ActiveCheckPoint.Value.Index < checkpoint.Index)
                {
                    ActiveCheckPoint.Value = checkpoint;

                    // If there are items in the temporary pickups list, we can remove them now
                    foreach (var pickUpItem in PickUpsTemp)
                    {
                        PickUps.Remove(pickUpItem);
                    }
                    PickUpsTemp.Clear();
                }

            };
        }

        /// <summary>
        /// ListItem notification. PickUpItem was added.
        /// </summary>
        /// <param name="pickUpItem">The pick up item.</param>
        private void PickUpItemAdded(PickUpItemBase pickUpItem)
        {
            pickUpItem.PickUpAction += () =>
            {
                pickUpItem.SetActive(false);
                PickUpsTemp.Add(pickUpItem);
                if (ItemPickUpAction != null)
                {
                    ItemPickUpAction();
                }   
            };
        }

        /// <summary>
        /// ListItem notification. PickUpItem was removed.
        /// </summary>
        /// <param name="pickUpItem">The pick up item.</param>
        private void PickUpItemRemoved(PickUpItemBase pickUpItem)
        {
            pickUpItem.DestroyModel();
        }

        /// <summary>
        /// ListItem notification. PickUpItem was removed from PickUpsTemp.
        /// </summary>
        /// <param name="pickUpItem">The pick up item.</param>
        private void PickUpTempItemRemoved(PickUpItemBase pickUpItem)
        {
            PickUps.Remove(pickUpItem);
        }

        /// <summary>
        /// Restores the PickUps from the PickUpsTemp list to the PickUps list.
        /// </summary>
        private void RestorePickUps()
        {
            foreach (var pickUpItemTemp in PickUpsTemp)
            {
                pickUpItemTemp.SetActive(true);
            }
            PickUpsTemp.Clear();
        }

        /// <summary>
        /// ListItem notification. Enemy was added.
        /// </summary>
        /// <param name="enemy">The enemy.</param>
        private void EnemyAdded(Enemy enemy)
        {
            enemy.OwnerGameLevel.Value = this;
            // Get notified of enemies target
            enemy.Target.OnValueChanged += (sender, value) =>
            {
                // Notify enemies if player is detected or not by other enemy
                if (PlayerIsDetectedAction != null)
                {
                    PlayerIsDetectedAction(CheckPlayerDetected());
                }
            };
            enemy.DamageableCurrentState.OnValueChanged += (sender, state) => OnEnemyStateChanged(enemy, state.Value);
        }

        /// <summary>
        /// Checks if the player detected by any enemy.
        /// </summary>
        /// <returns></returns>
        public bool CheckPlayerDetected()
        {
            return Enemies.Any(x => x.Target.Value != null);
        }

        /// <summary>
        /// Called when [enemy state changed].
        /// </summary>
        /// <param name="enemy">The enemy.</param>
        /// <param name="state">The state.</param>
        private void OnEnemyStateChanged(Enemy enemy, DamageableState state)
        {
            if (state == DamageableState.Destroyed)
            {
                if (enemy == Player.Value.Target.Value)
                {
                    Player.Value.Target.Value = null;
                }
                if (Enemies != null)
                {
                    if (Enemies.Contains(enemy))
                    {
                        Enemies.Remove(enemy);
                    }
                }
                // if an enemy died, check if still detected by any enemy
                if (PlayerIsDetectedAction != null)
                {
                    PlayerIsDetectedAction(CheckPlayerDetected());
                }                
            }
        }

        /// <summary>
        /// ListItem notification. Enemy was removed.
        /// </summary>
        /// <param name="enemy">The enemy.</param>
        private void EnemyRemoved(Enemy enemy)
        {
            // small delay to make sure any forces are applied before switch to ragdoll
            var timerComponent = ComponentExtensions.AddComponentOnEmptyChild<TimerTemporaryGameObject>(GameManager.TemporaryTimerComponents);
            timerComponent.TimerMethod = () =>
            {
                var charMotion = enemy.CurrentMotion.Value;
                var targetTransform = enemy.Transform;
                var enemyRagdollName = enemy.Name;
                enemy.DestroyModel();

                var ragdoll = new PlayerRagdoll(string.Format("{0}{1}", enemyRagdollName, "Ragdoll"));
                ragdoll.SetTransform(targetTransform);
                ragdoll.SetMotion(charMotion);
                Ragdolls.Add(ragdoll);
            };
            timerComponent.Invoke(0.1f);
            TimerComponents.Add(timerComponent);
        }
        #endregion
        #region TransitionToStates
        /// <summary>
        /// Transitions to state initialize.
        /// </summary>
        public void TransitionToStateInit()
        {
            GameLevelCurrentState.Value = GameLevelState.Start;
        }

        /// <summary>
        /// Transitions to state complete.
        /// </summary>
        public void TransitionToStateComplete()
        {
            if (GameLevelCurrentState.Value == GameLevelState.Running)
            {
                GameLevelCurrentState.Value = GameLevelState.Completed;
                if (LevelFinishedAction != null)
                {
                    LevelFinishedAction();
                }
            }

        }

        /// <summary>
        /// Transitions to state fail.
        /// </summary>
        public void TransitionToStateFail()
        {
            if (GameLevelCurrentState.Value == GameLevelState.Running
                ||
                GameLevelCurrentState.Value == GameLevelState.PlayerDead)
            {
                GameLevelCurrentState.Value = GameLevelState.Failed;
                if (LevelFailedAction != null)
                {
                    LevelFailedAction();
                }
            }
        }

        /// <summary>
        /// Transitions to state player death.
        /// </summary>
        public void TransitionToStatePlayerDeath()
        {
            if (GameLevelCurrentState.Value == GameLevelState.Running)
            {
                GameLevelCurrentState.Value = GameLevelState.PlayerDead;
                if (PlayerDeadAction != null)
                {
                    PlayerDeadAction();
                }
            }
        }

        /// <summary>
        /// Transitions to state run.
        /// </summary>
        public void TransitionToStateRun()
        {
            if (GameLevelCurrentState.Value == GameLevelState.Start
                ||
                GameLevelCurrentState.Value == GameLevelState.PlayerDead)
            {
                GameLevelCurrentState.Value = GameLevelState.Running;
                if (LevelRestartAction != null)
                {
                    LevelRestartAction();
                } 
                RestorePickUps();
            }
        }
        #endregion
    }
}
