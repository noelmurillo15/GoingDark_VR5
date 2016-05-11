using System;
using AGS.Core.Enums;
using AGS.Core.Systems.BaseSystem;

namespace AGS.Core.Systems.CharacterSystem
{
    [Serializable]
    public class DamageableResourceView : ActionView
    {
	
		#region Public properties
        public DamageableResourceType ResourceType;
        public int Max;
        public bool IsVital;
		#endregion

		public DamageableResource DamageableResource;

        #region ActionView functions
        public override void InitializeView()
        {
            DamageableResource = new DamageableResource(ResourceType, Max, IsVital);         
            SolveModelDependencies(DamageableResource);
        }
        #endregion ActionView

    }
}