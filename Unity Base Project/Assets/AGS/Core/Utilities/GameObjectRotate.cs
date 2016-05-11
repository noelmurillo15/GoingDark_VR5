using UnityEngine;

namespace AGS.Core.Utilities
{
    /// <summary>
    /// Simple GameObject rotation
    /// </summary>
    public class GameObjectRotate : MonoBehaviour
    {
        public Vector3 RotationVector;
        public float RotationSpeed;

        #region MonoBehaviours
        // override monobehaviours here
        void Update()
        {
            transform.Rotate(RotationVector * RotationSpeed * Time.deltaTime);
        }
        #endregion
    }
}
