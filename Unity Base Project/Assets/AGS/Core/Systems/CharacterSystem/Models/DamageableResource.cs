using AGS.Core.Classes.ActionProperties;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.CharacterSystem
{
    public class DamageableResource : ActionModel
    {
		#region Properties
		// Subscribable properties
		public ActionProperty<DamageableResourceType> ResourceType { get; private set; }
        public ActionProperty<int> Current { get; private set; }
        public ActionProperty<int> Max { get; private set; }
        public ActionProperty<bool> IsVital { get; private set; }
		#endregion Properties

		// Constructor
        public DamageableResource(DamageableResourceType resourceType, int max, bool isVital)
        {
            ResourceType = new ActionProperty<DamageableResourceType>() { Value = resourceType };
            Current = new ActionProperty<int> { Value = max };
            Current.OnValueChanged += (sender, current) =>
            {
                if (current.Value > max)
                {
                    Current.Value = max;
                }
            };
            Max = new ActionProperty<int> { Value = max };
            IsVital = new ActionProperty<bool> { Value = isVital };
		}
	}
}
