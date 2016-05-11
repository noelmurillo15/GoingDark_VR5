namespace AGS.Core.Systems.HazardSystem
{
    /// <summary>
    /// Hazard that kill instantly
    /// </summary>
	public class DeathTrap : HazardBase
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="DeathTrap"/> class.
        /// </summary>
        /// <param name="secondsActive">The seconds active duration of this hazard.</param>
        /// <param name="secondsRecharging">The seconds rechargingd uration of this hazard.</param>
        /// <param name="deactivateOnTrigger">if set to <c>true</c> [deactivate on trigger].</param>
        /// <param name="destroyOnTrigger">if set to <c>true</c> [destroy on trigger].</param>
        public DeathTrap(float secondsActive, float secondsRecharging, bool deactivateOnTrigger, bool destroyOnTrigger)
            : base(secondsActive, secondsRecharging, deactivateOnTrigger, destroyOnTrigger)
        {

		}


	}
}
