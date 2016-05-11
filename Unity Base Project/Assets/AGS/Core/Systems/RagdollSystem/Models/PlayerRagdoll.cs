namespace AGS.Core.Systems.RagdollSystem
{
    /// <summary>
    /// Just a ragdoll that can be identified as the players ragdoll
    /// </summary>
	public class PlayerRagdoll : Ragdoll {

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerRagdoll"/> class.
        /// </summary>
        /// <param name="prefabName">Name of the ragdoll prefab.</param>
		public PlayerRagdoll (string prefabName) 
            : base(prefabName)
		{
			
		}
	}
}
