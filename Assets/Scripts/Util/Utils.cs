﻿using System;
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
        /// Get the magnitude using quake's fast inverse square root
        /// </summary>
        public static float FastInvMag(this Vector3 self)
        {
            return Q_rsqrt(self.sqrMagnitude);
        }

        
        /// <summary>
        /// Get the magnitude using quake's fast inverse square root
        /// </summary>
        public static float FastMag(this Vector3 self)
        {
            var mag = self.sqrMagnitude;
            return Q_rsqrt(mag) * mag;
        }
        
        
        /// <summary>
        /// Set a specific value of the vector
        /// </summary>
        public static Quaternion Quaternion(float? x = null, float? y = null, float? z = null)
        {
            return UnityEngine.Quaternion.Euler(x ?? 0, y ?? 0, z ?? 0);
        }
        
        
        /// <summary>
        /// Set a specific value of the vector using Quake inverse square root
        /// from: https://stackoverflow.com/questions/268853/is-it-possible-to-write-quakes-fast-invsqrt-function-in-c
        /// </summary>
        public static float Q_rsqrt(float number)
        {
            int i;
            float x2, y;
            const float threehalfs = 1.5f;

            x2 = number * 0.5f;
            y = number;
            i = BitConverter.ToInt32(BitConverter.GetBytes(y), 0);     // evil floating point bit level hacking
            i = 0x5f3759df - (i >> 1);                                             // what the fuck? 
            y = BitConverter.ToSingle(BitConverter.GetBytes(i), 0);
            y = y * (threehalfs - x2 * y * y);                                     // 1st iteration
            // y = y * (threehalfs - x2 * y * y);                                  // 2st iteration, this can be removed
            
            return y;
        }
    }
}