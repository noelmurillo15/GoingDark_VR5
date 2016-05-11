using UnityEngine;

namespace AGS.Core.Systems.CharacterSystem
{
    /// <summary>
    /// Enemy is a straight character implementation. To create an andvanced enemy, inherit from AdvancedCharacter.
    /// </summary>
	public class Enemy : CharacterBase {

        /// <summary>
        /// Initializes a new instance of the <see cref="Enemy"/> class.
        /// </summary>
        /// <param name="transform">The CharacterBase transform.</param>
        /// <param name="name">CharacterBase name. Is instantiating ragdolls upon death, this need to match Ragdoll prefix. A Killable with the name John will try to instantiate JohnRagdoll</param>
        /// <param name="speed">General speed of the character</param>
        ///         /// <param name="turnSpeed">Turn speed of the character</param>
        /// <param name="skinWidth">SkinWidth determines how far out from Character Collider raycasts that determines environment detection should go</param>
        /// <param name="skinCorrectionRays">How many rays should be used for detecting environment</param>
        /// <param name="slopeLimitMoving">Slope angle treshold in degrees before character begins to slide on slopes</param>
        /// <param name="slopeLimitSliding">Slope angle treshold in degress that determines if character should crouch or start manual sliding</param>
        public Enemy(Transform transform, string name, float speed, float turnSpeed, float skinWidth, int skinCorrectionRays, float slopeLimitMoving, float slopeLimitSliding)
            : base(transform, name, speed, turnSpeed, skinWidth, skinCorrectionRays, slopeLimitMoving, slopeLimitSliding)
	    {
	    }
	}
}
