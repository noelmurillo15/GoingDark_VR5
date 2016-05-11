using System;
using System.Collections.Generic;
using AGS.Core.Enums;
using UnityEngine;

namespace AGS.Core.Base
{
    /// <summary>
    /// GameSettings
    /// </summary>
    [Serializable]
    public class GameSettings : MonoBehaviour
    {
        /// <summary>
        /// Gets a value indicating whether [show combat text].
        /// </summary>
        /// <value>
        ///   <c>true</c> if should [show combat text]; otherwise, <c>false</c>.
        /// </value>
        public static bool ShowCombatText
        {
            get { return Convert.ToBoolean(PlayerPrefs.GetInt("Hide Combat Text", 1)); }
            private set { PlayerPrefs.SetInt("Hide Combat Text", Convert.ToInt32(value)); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [hide tutorial help].
        /// </summary>
        /// <value>
        ///   <c>true</c> if should [hide tutorial help]; otherwise, <c>false</c>.
        /// </value>
        public static bool HideTutorialHelp
        {
            get { return Convert.ToBoolean(PlayerPrefs.GetInt("Hide Tutorial Help", 0)); }
            set { PlayerPrefs.SetInt("Hide Tutorial Help", Convert.ToInt32(value)); }
        }

        /// <summary>
        /// Gets the difficulty. /* This setting is currently not in use */
        /// </summary>
        /// <value>
        /// The difficulty.
        /// </value>
        public GameDifficulty Difficulty { get; set; }

        /// <summary>
        /// Gets the starting lives.
        /// </summary>
        /// <value>
        /// The starting lives.
        /// </value>
        public static int StartingLives
        {
            get { return PlayerPrefs.GetInt("Starting Lives", 10); }
            private set { PlayerPrefs.SetInt("Starting Lives", value); }
        }

        /// <summary>
        /// Gets number of throwables to start with if throwing skill is in use.
        /// </summary>
        /// <value>
        /// The starting throwables.
        /// </value>
        public static int StartingThrowables
        {
            get { return PlayerPrefs.GetInt("Starting Throwables", 10); }
            private set { PlayerPrefs.SetInt("Starting Throwables", value); }
        }

        public ThrowableWeaponType[] ThrowableWeaponTypes { get; set; }
        public int[] ThrowableWeaponCount { get; set; }

        /// <summary>
        /// If using throwing skill and player should start with throwables, override this class to setup StartingThrowablesDictionary with type and count.
        /// </summary>
        /// <value>
        /// The starting throwables dictionary.
        /// </value>
        public static Dictionary<ThrowableWeaponType, int> StartingThrowablesDictionary { get; set; }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void SaveSettings()
        {
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Sets the player prefs.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void SetPlayerPrefsInt(string key, int value)
        {
            switch (key)
            {
                case "Starting Lives":
                    StartingLives = value;
                    break;
                case "Starting Throwables":
                    StartingThrowables = value;
                    break;
            }
        }

        /// <summary>
        /// Sets the show combat text.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public void SetShowCombatText(bool value)
        {
            ShowCombatText = value;
        }
    }
}
