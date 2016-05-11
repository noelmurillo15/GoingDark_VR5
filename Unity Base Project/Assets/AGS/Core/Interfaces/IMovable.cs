using AGS.Core.Systems.StatusEffectSystem;
using UnityEngine;

namespace AGS.Core.Interfaces
{
    /// <summary>
    /// Moving models should implement this interface
    /// </summary>
    public interface IMovable
    {
        Transform Transform {get;set;} 
		void ApplyPushEffect(PushEffect pushEffect);
        void ApplyPushEffect(PushEffect pushEffect, bool hitFromBehind);
    }
}
