using AGS.Core.Classes.ActionProperties;

namespace AGS.Core.Base
{
    /// <summary>
    /// Data class used for saving game progress etc 
    /// </summary>
    public class GameData
    {
        public ActionProperty<int> PlayerLives;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameData"/> class.
        /// </summary>
        public GameData()
        {
            PlayerLives = new ActionProperty<int>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameData"/> class.
        /// </summary>
        /// <param name="startingLives">The starting lives.</param>
        public GameData(int startingLives)
        {
            PlayerLives = new ActionProperty<int> {Value = startingLives};
        }

        /// <summary>
        /// Increases the lives.
        /// </summary>
        public void IncreaseLives()
        {
            PlayerLives.Value++;
        }

        /// <summary>
        /// Decreases the lives.
        /// </summary>
        public void DecreaseLives()
        {
            PlayerLives.Value--;
        }
    }
}
