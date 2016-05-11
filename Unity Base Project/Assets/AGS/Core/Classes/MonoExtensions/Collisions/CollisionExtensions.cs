using System;
using UnityEngine;

namespace AGS.Core.Classes.MonoExtensions
{
    /// <summary>
    /// Use these function to detect collisions with a gameObject that has a component of type T.
    /// Enter, Stay, Exit has identical functionality as corresponding monobehaviour
    /// </summary>
    public static class CollisionExtensions
    {
        /// <summary>
        /// Called when [collision action enter with] T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject">The game object.</param>
        /// <param name="action">The action.</param>
        public static void OnCollisionActionEnterWith<T>(this GameObject gameObject, Action<T> action)
            where T : MonoBehaviour
        {
            ComponentExtensions.SetupComponent<CollisionAction>(gameObject).OnCollisionEnterAction +=
                other => InvokeCollisionAction(other, action);
        }

        /// <summary>
        /// Called when [collision action stay with] T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject">The game object.</param>
        /// <param name="action">The action.</param>
        public static void OnCollisionActionStayWith<T>(this GameObject gameObject, Action<T> action)
            where T : MonoBehaviour
        {
            ComponentExtensions.SetupComponent<CollisionAction>(gameObject).OnCollisionStayAction +=
                other => InvokeCollisionAction(other, action);
        }

        /// <summary>
        /// Called when [collision action exit with] T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject">The game object.</param>
        /// <param name="action">The action.</param>
        public static void OnCollisionActionExitWith<T>(this GameObject gameObject, Action<T> action)
            where T : MonoBehaviour
        {
            ComponentExtensions.SetupComponent<CollisionAction>(gameObject).OnCollisionExitAction +=
                other => InvokeCollisionAction(other, action);
        }

        /// <summary>
        /// Invokes the collision action with T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="other">The other.</param>
        /// <param name="action">The action.</param>
        private static void InvokeCollisionAction<T>(Collision other, Action<T> action) where T : MonoBehaviour
        {
            if (action != null && other.gameObject.GetComponent<T>() != null)
            {
                action.Invoke(other.gameObject.GetComponent<T>());
            }
        }
    }
}
