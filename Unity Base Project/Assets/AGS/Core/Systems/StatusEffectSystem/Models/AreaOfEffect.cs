using AGS.Core.Classes.ActionProperties;
using AGS.Core.Interfaces;
using AGS.Core.Systems.BaseSystem;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.RagdollSystem;

namespace AGS.Core.Systems.StatusEffectSystem
{
    /// <summary>
    /// An AreaOfEffect checks for possible victimes within its volume
    /// </summary>
	public class AreaOfEffect : ActionModel
	{
        #region Properties
		// Subscribable properties
        public ActionList<KillableBase> KillableTargets { get; private set; } // Current Killables within volume
        public ActionList<Ragdoll> RagdollTargets { get; private set; } // Current Ragdolls within volume
        public ActionList<IMovable> MovableTargets { get; private set; } // Current Movables within volume
		#endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="AreaOfEffect"/> class.
        /// </summary>
		public AreaOfEffect () {
            KillableTargets = new ActionList<KillableBase>();
            RagdollTargets = new ActionList<Ragdoll>();
            MovableTargets = new ActionList<IMovable>();
		}
	}
}
