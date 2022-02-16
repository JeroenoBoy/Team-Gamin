using System;
using UnityEngine;

namespace Util
{
    public static class Utils
    {
        /// <summary>
        /// Set a specific value of the vector
        /// </summary>
        public static Vector3 With(this Vector3 self, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? self.x, y ?? self.y, z ?? self.z);
        }
        
        
        /// <summary>
        /// Set a specific value of the vector
        /// </summary>
        public static Vector3 ClampMagnitude(this Vector3 self, float maxForce)
        {
            return Vector3.ClampMagnitude(self, maxForce);
        }
        
        
        /// <summary>
        /// Set a specific value of the vector
        /// </summary>
        public static Ray ForwardRay(this Component self)
        {
            var transform = self.transform;
            return new Ray(transform.position, transform.forward);
        }
        
        
        /// <summary>
        /// Set a specific value of the vector
        /// </summary>
        public static Quaternion Quaternion(float? x = null, float? y = null, float? z = null)
        {
            return UnityEngine.Quaternion.Euler(x ?? 0, y ?? 0, z ?? 0);
        }


        /// <summary>
        /// Compare if a gameObject has a layer
        /// </summary>
        public static bool HasLayer(this Component comp, int layer)
        {
            var gameObject = comp.gameObject;
            return ((1 << gameObject.layer) & layer) != 0 || layer == gameObject.layer;
        }
    }
}