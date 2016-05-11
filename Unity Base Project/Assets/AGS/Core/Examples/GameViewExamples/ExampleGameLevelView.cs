using System;
using AGS.Core.Systems.GameLevelSystem;

namespace AGS.Core.Examples.GameViewExamples
{
    /// <summary>
    /// ExampleGameLevelView
    /// </summary>
    [Serializable]
    public class ExampleGameLevelView : GameLevelBaseView
    {
        public override void InitializeView()
        {
            GameLevel = new GameLevel(StartDelaySeconds);
            SolveModelDependencies(GameLevel);
        }
    }
}