namespace AGS.Core.Classes.TimerComponents
{
    /// <summary>
    /// This component will destroy the GameObject it is attached to when finished
    /// </summary>
    public class FixedUpdateTemporaryGameObject : FixedUpdateComponent
    {
        void OnEnable()
        {
            OnFinishedAction = () =>
            {
                if (gameObject != null)
                {
                    Destroy(gameObject);    
                }
                
            }; 
        }
    }
}
