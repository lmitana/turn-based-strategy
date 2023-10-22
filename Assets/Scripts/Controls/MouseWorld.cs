using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Turnbased.Controls
{
    /// <summary>
    /// This component handles mouse pointer in the 3D space.    
    /// Must be attached on a gameObject
    /// </summary>
    public class MouseWorld : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance
        /// </summary>
        static MouseWorld instance;

        /// <summary>
        /// Layer for the raycaster hit detection
        /// </summary>
        [field: SerializeField] public LayerMask MousePlane { get; private set; }

        /// <summary>
        /// Mouse pointer coordinates in world space
        /// </summary>
        [field: SerializeField] public Vector3 WorldPosition { get; private set; }

        void Awake()
        {
            instance = this;
        }

        /// <summary>
        /// Sync gameobject position with mouse screen position
        /// </summary>
        void Update()
        {
            transform.position = MouseWorld.GetPosition();
        }

        /// <summary>
        /// Calculate mouse pointer position in world space
        /// </summary>
        /// <returns>Position in world space</returns>
        public static Vector3 GetPosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
            Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance.MousePlane);
            return raycastHit.point;
        }
    }
}