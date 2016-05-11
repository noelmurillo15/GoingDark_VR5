using UnityEngine;

namespace AGS.Core.Classes.MonoExtensions
{
    /// <summary>
    /// MonoBehaviour extension methods
    /// </summary>
    public class ComponentExtensions : MonoBehaviour
    {
        /// <summary>
        /// Creates a component of type T to gameObject parameter and returns it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject">The game object.</param>
        /// <returns></returns>
        public static bool HasComponent<T>(GameObject gameObject) where T : MonoBehaviour
        {
            return gameObject.GetComponent<T>() != null;
        }

        /// <summary>
        /// Get a component of type T from gameObject parameter. Creates component T if it is missing.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject">The game object.</param>
        /// <returns></returns>
        public static T SetupComponent<T>(GameObject gameObject) where T : MonoBehaviour
        {
            var component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();            
            }
            return component;
        }

        /// <summary>
        /// Creates a component of type T to gameObject parameter and returns it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject">The game object.</param>
        /// <returns></returns>
        public static T AddComponent<T>(GameObject gameObject) where T : MonoBehaviour
        {
            return gameObject.AddComponent<T>();
        }

		/// <summary>
		/// Creates a component of type T on a new child to gameObject parameter and returns it.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="gameObject">The game object.</param>
		/// <param name="childName">Name of the child.</param>
		/// <returns></returns>
		public static T AddComponentOnEmptyChild<T>(GameObject gameObject) where T : MonoBehaviour
		{
			return AddComponentOnEmptyChild<T> (gameObject, "");
		}

        /// <summary>
        /// Creates a component of type T on a new child to gameObject parameter and returns it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject">The game object.</param>
        /// <param name="childName">Name of the child.</param>
        /// <returns></returns>
        public static T AddComponentOnEmptyChild<T>(GameObject gameObject, string childName) where T : MonoBehaviour
        {
            var go = new GameObject(childName);
            go.transform.parent = gameObject.transform;
            go.transform.localPosition = Vector3.zero;
            return go.AddComponent<T>();
        }

        /// <summary>
        /// Creates a component of type T on gameObject parameter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject">The game object.</param>
        /// <param name="component">The component.</param>
        public static void RemoveComponent<T>(GameObject gameObject, T component) where T : MonoBehaviour
        {
            Destroy(gameObject.GetComponent<T>());
        }
    }
}
