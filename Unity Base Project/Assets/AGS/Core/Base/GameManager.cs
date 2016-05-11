using System;
using System.Linq;
using AGS.Core.Classes.ActionProperties;
using AGS.Core.Classes.DataClasses;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.GameLevelSystem;
using UnityEngine;

namespace AGS.Core.Base
{
    /// <summary>
    /// Main class to be used on root object in any AGS scene.
    /// Responsible for initializing all views, intital setups, dynamically added views, handling game state and making sure that a player is present
    /// </summary>
    public class GameManager : MonoBehaviour
    {

        public GameLevel GameLevel;
        public PlayerBaseView PlayerView { get; private set; }
        public ActionProperty<Player> Player;
        public static GameSettings GameSettings;
        public static GameData GameData;
        public UnityEngine.Object PlayerPrefab;

        /// <summary>
        /// TemporaryTimerComponents is mainly used by game models to attach timer components. (Models are not MonoBehaviours and does not have any knowledge of the unity scene or objects)
        /// </summary>
        public static GameObject TemporaryTimerComponents;

        public static GameObject EffectsContainer;
        public static GameObject ProjectilesContainer;

        /// <summary>
        /// Static list of all AGS views present in the scene. See ActionList class & ActionView for detailed info.
        /// </summary>
        public static ActionList<ActionView> ActionViews;

        /// <summary>
        /// This action should be called when scene is ready to run. I.E all ActionViews are ready.
        /// </summary>
        public Action OnLevelInitialized { get; set; }


        void Awake()
        {
            TemporaryTimerComponents = GameObject.Find("TemporaryTimerComponents") ?? gameObject; // If missing a TemporaryTimerComponents we add temp timers directly on the GameManager
            EffectsContainer = GameObject.Find("Effects") ?? gameObject; // If missing an EffectsContainer we add effects directly on the GameManager
            ProjectilesContainer = GameObject.Find("Projectiles") ?? gameObject; // If missing an ProjectilesContainer we add projectiles directly on the GameManager
            GameSettings = GetComponentInChildren<GameSettings>();
            ActionViews = GetComponentsInChildren<ActionView>().ToActionList(); // Get all ActionVies. Scene may very well have tons of other objects in the same heirarchy, wich is perfectly ok.
            ActionViews.Reverse(); // Reverse the list to make views lowest in heirarchy get initialized first
            Player = new ActionProperty<Player>();
            GameData = new GameData(GameSettings.StartingLives);
        }

        void Start()
        {
            /* Subscribe to player lives for game state updating */
            GameData.PlayerLives.OnValueChanged += (sender, remainingLives) => OnRemainingLivesChanged(remainingLives.Value);
 
            foreach (var view in ActionViews)
            {
                view.InitializeView();
            }
            foreach (var view in ActionViews)
            {
                view.InitializeActionModel(view.ActionModel);
            }
            SetGameLevel();
            /* Subscribe to any new views that are added to the scene at runtime, and initialize them */
            ActionViews.ListItemAdded += OnActionViewDynamicAdd;

            if (GameLevel != null)
            {
                /* Subscribe to the GameLevelState */
                GameLevel.GameLevelCurrentState.OnValueChanged += (sender, state) => OnGameLevelStateChanged(state.Value);
            }

        }

        /// <summary>
        /// Called when [game level state changed].
        /// </summary>
        /// <param name="gameLevelState">State of the game level.</param>
        private void OnGameLevelStateChanged(GameLevelState gameLevelState)
        {
            switch (gameLevelState)
            {
                case GameLevelState.Running:
                    CreatePlayer();
                    break;
                case GameLevelState.Completed:
                    break;
                case GameLevelState.PlayerDead:
                    GameData.DecreaseLives();
                    break;
            }
        }

        /// <summary>
        /// Sets the game level.
        /// </summary>
        private void SetGameLevel()
        {
            /* Get and cache the reference to the GameLevel */
            var gameLevelView = ActionViews.FirstOrDefault(x => x as GameLevelBaseView) as GameLevelBaseView;
            if (gameLevelView == null) return;
            GameLevel = gameLevelView.GameLevel;
        }

        /// <summary>
        /// Creates the player.
        /// </summary>
        private void CreatePlayer()
        {
            /* Try to find a present player, create new if missing. Call PlayerSetup when PlayerView is ViewReady */
            PlayerView = ActionViews.FirstOrDefault(x => x as PlayerBaseView) as PlayerBaseView;
            if (PlayerView == null)
            {
                var playerObj = Instantiate(PlayerPrefab) as GameObject;
                if (playerObj == null) return;
                PlayerView = playerObj.GetComponent<PlayerBaseView>();

                if (PlayerView == null) return;
                PlayerView.transform.parent = transform;
                if (PlayerView.ViewReady.Value)
                {
                    PlayerSetup();
                }
                else
                {
                    PlayerView.ViewReady.OnValueChanged += (sender, e) =>
                    {
                        if (!e.Value) return;
                        PlayerSetup();
                    };
                }
            }
            else
            {
                if (PlayerView.ViewReady.Value)
                {
                    PlayerSetup();
                }
                else
                {
                    PlayerView.ViewReady.OnValueChanged += (sender, viewReady) =>
                    {
                        if (!viewReady.Value) return;
                        PlayerSetup();
                    };
                }
            }

        }

        /// <summary>
        /// Setup the player and add possible starting throwables.
        /// </summary>
        private void PlayerSetup()
        {
            /* Cache a reference to the Player and call AddStartingThrowables to add any initial throwables */
            Player.Value = PlayerView.Player;
            AddStartingThrowables();
            GameLevel.Player.Value = Player.Value;
        }

        /// <summary>
        /// Adds the starting throwables.
        /// </summary>
        private void AddStartingThrowables()
        {
            if (GameSettings == null || !GameSettings.StartingThrowablesDictionary.Any()) return;
            foreach (var startingThrowable in GameSettings.StartingThrowablesDictionary)
            {
                if (Player.Value.ThrowableWeaponStashes.Any(x => x.ThrowableWeaponType == startingThrowable.Key)) return;
                var throwableWeaponStash = new ThrowableWeaponStash()
                {
                    ThrowableWeaponType = startingThrowable.Key,
                    Count = new ActionProperty<int>() { Value = startingThrowable.Value }
                };
                Player.Value.ThrowableWeaponStashes.Add(throwableWeaponStash);
            }
        }

        /// <summary>
        /// This function handles dynamically added ActionViews. Whenever a new view is added to the scene (must be placed under the root GameManager) this function will be called.
        /// ActionViews that have child objects with other ActionViews will wait for ViewReady on childs before initializing.
        /// </summary>
        /// <param name="actionViewAdded">The action view added.</param>
        private void OnActionViewDynamicAdd(ActionView actionViewAdded)
        {
            var childViews = actionViewAdded.GetComponentsInChildren<ActionView>().Where(childView => childView.enabled).ToList();
            childViews.Remove(actionViewAdded);
            if (childViews.Count == 0)
            {
                SetupDynamicActionView(actionViewAdded);
            }
            else
            {
                foreach (var childView in childViews)
                {
                    childView.ViewReady.OnValueChanged += (sender, e) =>
                    {
                        if (!e.Value) return;
                        if (childViews.All(x => x.ViewReady.Value))
                        {
                            SetupDynamicActionView(actionViewAdded);
                        }
                    };
                }
            }
        }

        /// <summary>
        /// Setups the dynamic action view.
        /// </summary>
        /// <param name="actionViewAdded">The action view added.</param>
        private static void SetupDynamicActionView(ActionView actionViewAdded)
        {
            actionViewAdded.InitializeView();
            actionViewAdded.InitializeActionModel(actionViewAdded.ActionModel);
        }

        /// <summary>
        /// Called when [remaining lives changed].
        /// Transition GameLevel to fail if no lives left´.
        /// </summary>
        /// <param name="remainingLives">The remaining lives.</param>
        private void OnRemainingLivesChanged(int remainingLives)
        {
            if (remainingLives <= 0)
            {
                GameLevel.TransitionToStateFail();
            }
        }
    }
}
