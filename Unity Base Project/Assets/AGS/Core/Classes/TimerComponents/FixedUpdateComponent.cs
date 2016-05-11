namespace AGS.Core.Classes.TimerComponents
{
    /// <summary>
    /// Base FixedUpdateComponent will call TimerMethod each FixedUpdate
    /// </summary>
    public abstract class FixedUpdateComponent : UpdateComponentBase
    {
        void FixedUpdate()
        {
            if (UpdateMethod != null)
            {
                UpdateMethod();
            }
        }
    }
}
