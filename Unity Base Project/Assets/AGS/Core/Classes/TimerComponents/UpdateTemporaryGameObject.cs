namespace AGS.Core.Classes.TimerComponents
{
    /// <summary>
    /// This component will destroy the GameObject it is attached to when finished
    /// </summary>
    public class UpdateTemporaryGameObject : UpdateComponent
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

        void OnDestroy()
        {
            OnFinishedAction = null;
        }
    }
}
