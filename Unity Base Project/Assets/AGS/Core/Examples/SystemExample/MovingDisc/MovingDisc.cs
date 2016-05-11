using AGS.Core.Classes.ActionProperties;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Examples.SystemExample
{
    public enum MovingDiscDirection {
        Forward,
        Backward,
        None
    }
    public class MovingDisc : ActionModel
    {
        #region Properties
        // Subscribable properties
        public ActionProperty<MovingDiscDirection> CurrentDirection { get; set; }
        #endregion Properties

        public MovingDisc()
        {
            CurrentDirection = new ActionProperty<MovingDiscDirection>();
        }

        #region public functions

        public void TransitionToStateForward(){
            CurrentDirection.Value = MovingDiscDirection.Forward;
        }
        public void TransitionToStateBackward(){
            CurrentDirection.Value = MovingDiscDirection.Backward;
        }
        public void TransitionToStateStop(){
            CurrentDirection.Value = MovingDiscDirection.None;
        }
        #endregion
    }
}
