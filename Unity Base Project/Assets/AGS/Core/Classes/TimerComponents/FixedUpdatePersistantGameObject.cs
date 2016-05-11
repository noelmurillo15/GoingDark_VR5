namespace AGS.Core.Classes.TimerComponents
{
    /// <summary>
    /// This component will only remove itself when finished. The GameObject it is attached to will remain.
    /// </summary>
    public class FixedUpdatePersistantGameObject : FixedUpdateComponent
    {
        void OnEnable()
        {
            OnFinishedAction = () => MonoExtensions.ComponentExtensions.RemoveComponent(gameObject, this); 
        }
    }
}
