using System;
using UnityEngine;

namespace AGS.Core.Classes.MonoExtensions
{
    /// <summary>
    /// Use these function to detect triggers with a gameObject that has a component of type T.
    /// Enter, Stay, Exit has identical functionality as corresponding monobehaviour
    /// </summary>
    public static class TriggerExtensions 
    {
        /// <summary>
        /// Called when [trigger action enter with] T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject">The game object.</param>
        /// <param name="action">The action.</param>
        public static void OnTriggerActionEnterWith<T>(this GameObject gameObject, Action<T> action) where T : MonoBehaviour
        {
            ComponentExtensions.SetupComponent<TriggerAction>(gameObject).OnTriggerEnterAction +=
                other => InvokeTriggerAction(other, action);
        }

        /// <summary>
        /// Called when [trigger action stay with] T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject">The game object.</param>
        /// <param name="action">The action.</param>
        public static void OnTriggerActionStayWith<T>(this GameObject gameObject, Action<T> action) where T : MonoBehaviour
        {
            ComponentExtensions.SetupComponent<TriggerAction>(gameObject).OnTriggerStayAction +=
                other => InvokeTriggerAction(other, action);
        }

        /// <summary>
        /// Called when [trigger action exit with] T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject">The game object.</param>
        /// <param name="action">The action.</param>
        public static void OnTriggerActionExitWith<T>(this GameObject gameObject, Action<T> action) where T : MonoBehaviour
        {
            ComponentExtensions.SetupComponent<TriggerAction>(gameObject).OnTriggerExitAction +=
                other => InvokeTriggerAction(other, action);
        }

        /// <summary>
        /// Invokes the trigger action with T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="other">The other.</param>
        /// <param name="action">The action.</param>
        private static void InvokeTriggerAction<T>(Collider other, Action<T> action) where T : MonoBehaviour
        {
            if (action != null && other.gameObject.GetComponent<T>() != null)
            {
                action(other.gameObject.GetComponent<T>());
            }
        }
    }
}
