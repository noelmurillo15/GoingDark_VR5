using System;
using UnityEngine;

namespace AGS.Core.Classes.MonoExtensions
{
    /// <summary>
    /// This compononent is added automatically by TriggerExtensions when using any of its functions
    /// </summary>
    [DisallowMultipleComponent]
    public class TriggerAction : MonoBehaviour
    {
        public Action<Collider> OnTriggerEnterAction;
        public Action<Collider> OnTriggerStayAction;
        public Action<Collider> OnTriggerExitAction;

        void OnTriggerEnter(Collider other)
        {
            if (OnTriggerEnterAction != null)
            {
                OnTriggerEnterAction(other);
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (OnTriggerStayAction != null)
            {
                OnTriggerStayAction(other);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (OnTriggerExitAction != null)
            {
                OnTriggerExitAction(other);
            }
        }
    }
}
