using System;
using UnityEngine;

namespace Controllers.Paths
{
    //  This class is simply a tag with some utils to make code shorter
    
    public class PathNode : MonoBehaviour
    {

        /// <summary>
        /// Get the current position
        /// </summary>
        public Vector3 Position => transform.position;
        public int Index;


        #if UNITY_EDITOR
        //  More editor only code that could screw with performance

        private PathController _controller;
        private void OnValidate()
        {
            if (!_controller) _controller = GetComponentInParent<PathController>();
        
            if(_controller.Contains(this)) return;
            _controller.NodeAdded();
        }

        #endif
    }
}