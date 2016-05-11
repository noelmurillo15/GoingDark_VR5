using System;
using UnityEngine;

namespace AGS.Core.Classes.MonoExtensions
{
    /// <summary>
    /// This compononent is added automatically by CollisionExtensions when using any of its functions
    /// </summary>
    [DisallowMultipleComponent]
    public class CollisionAction : MonoBehaviour
    {
        public Action<Collision> OnCollisionEnterAction;
        public Action<Collision> OnCollisionStayAction;
        public Action<Collision> OnCollisionExitAction;

        void OnCollisionEnter(Collision collision)
        {
            if (OnCollisionEnterAction != null)
            {   
                OnCollisionEnterAction(collision);
            }            
        }

        void OnCollisionStay(Collision collision)
        {
            if (OnCollisionStayAction != null)
            {
                OnCollisionStayAction(collision);
            }
        }

        void OnCollisionExit(Collision collision)
        {
            if (OnCollisionExitAction != null)
            {
                OnCollisionExitAction(collision);
            }
        }
    }
}
