using AGS.Core.Classes.CollisionReferences;
using AGS.Core.Enums;
using AGS.Core.Systems.CharacterSystem;
using AGS.Core.Systems.InteractionSystem;
using AGS.Core.Systems.InteractionSystem.Interactables;
using AGS.Core.Systems.MovingEnvironmentSystem;
using AGS.Core.Systems.RagdollSystem;
using UnityEngine;

namespace AGS.Core.Classes.Helpers
{
    /// <summary>
    /// Static helper to get HitObstacle from a Transform
    /// </summary>
    public static class HitObstacleHelper
    {
        /// <summary>
        /// Gets the hit obstacle.
        /// </summary>
        /// <param name="hitTransform">The hit transform.</param>
        /// <returns></returns>
        public static HitObstacle GetHitObstacle(Transform hitTransform)
        {
            if (MonoExtensions.ComponentExtensions.HasComponent<RampTop>(hitTransform.gameObject))
            {
                return HitObstacle.RampTop;
            }
            if (MonoExtensions.ComponentExtensions.HasComponent<Stairs>(hitTransform.gameObject))
            {
                return HitObstacle.Stairs;
            }
            if (MonoExtensions.ComponentExtensions.HasComponent<MovingEnvironmentBaseView>(hitTransform.gameObject))
            {
                return HitObstacle.MovingGround;
            }
            if (MonoExtensions.ComponentExtensions.HasComponent<LadderTop>(hitTransform.gameObject))
            {
                return HitObstacle.LadderTop;
            }
            if (MonoExtensions.ComponentExtensions.HasComponent<LadderStand>(hitTransform.gameObject))
            {
                return HitObstacle.LadderStand;
            }
            if (MonoExtensions.ComponentExtensions.HasComponent<LadderStand>(hitTransform.gameObject))
            {
                return HitObstacle.LadderStand;
            }
            if (MonoExtensions.ComponentExtensions.HasComponent<MovableObjectBaseView>(hitTransform.gameObject))
            {
                return HitObstacle.Movable;
            }
            if (MonoExtensions.ComponentExtensions.HasComponent<CharacterBaseView>(hitTransform.gameObject))
            {
                return HitObstacle.Character;
            }
            if (MonoExtensions.ComponentExtensions.HasComponent<RagdollView>(hitTransform.gameObject))
            {
                return HitObstacle.Ragdoll;
            }
            return HitObstacle.Ground;
        }
    }

}
