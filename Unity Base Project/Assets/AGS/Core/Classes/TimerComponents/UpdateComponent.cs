namespace AGS.Core.Classes.TimerComponents
{
    /// <summary>
    /// Base UpdateComponent will call TimerMethod each Update
    /// </summary>
    public abstract class UpdateComponent : UpdateComponentBase
    {
        void Update()
        {
            if (UpdateMethod != null)
            {
                UpdateMethod();
            }
        }
    }
}
