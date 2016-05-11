namespace AGS.Core.Classes.TimerComponents
{
    /// <summary>
    /// This component will destroy the GameObject it is attached to when finished
    /// </summary>
    public class TimerTemporaryGameObject : TimerComponent
    {
        void OnEnable()
        {
            OnFinishedAction = () => Destroy(gameObject); 
        }

    }
}
