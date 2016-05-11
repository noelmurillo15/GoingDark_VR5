using System;

namespace AGS.Core.Systems.CharacterSystem
{
    /// <summary>
    /// Like CharacterBaseView with an enemy model.
    /// </summary>
    [Serializable]
    public abstract class EnemyBaseView : CharacterBaseView
    {
        #region Public properties
        public Enemy Enemy;
        #endregion

        #region AGS Setup
        public override void InitializeView()
        {
            Enemy = new Enemy(transform, Name, Speed, TurnSpeed, SkinWidth, SkinCorrectionRays, SlopeLimitMoving, SlopeLimitSliding);
            SolveModelDependencies(Enemy);
        }

        #endregion
    }
}