using UnityEditor;
using UnityEngine;

namespace Util
{
    public static class DisplayVector
    {
        /// <summary>
        /// Draws a ray with a disc relative to the position and the input ray
        /// </summary>
        public static void Draw(Vector3 position, Vector3 Vector, Color color)
        {
#if UNITY_EDITOR
            var target = position + Vector;
            Handles.color = color;
            Handles.DrawLine(position, target);
            Handles.DrawSolidDisc(target, Vector3.up, 0.3f);
#endif
        }
        
    }
}