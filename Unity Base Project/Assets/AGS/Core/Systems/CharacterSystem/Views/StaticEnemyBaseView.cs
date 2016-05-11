using System;

namespace AGS.Core.Systems.CharacterSystem
{
    /// <summary>
    /// Like CharacterBaseView with an enemy model.
    /// </summary>
    [Serializable]
    public abstract class StaticEnemyBaseView : CombatEntityBaseView
    {
        #region Public properties
        public StaticEnemy StaticEnemy;
        #endregion

        #region AGS Setup
        public override void InitializeView()
        {
            StaticEnemy = new StaticEnemy(transform, Name, TurnSpeed);
            SolveModelDependencies(StaticEnemy);
        }

        #endregion
    }
}