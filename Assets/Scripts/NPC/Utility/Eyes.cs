using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Util;

namespace NPC.Utility
{
    public class Eyes : MonoBehaviour
    {
        [SerializeField] private float     _fov  = 70;
        [SerializeField] private int       _rays = 30;
        [SerializeField] private int       _rayLength = 30;
        [SerializeField] private LayerMask _interactMask;


        private void OnValidate()
        {
            if (_fov       < 1) _fov = 1;
            if (_rays      < 1) _rays = 1;
            if (_rayLength < 0) _rayLength = 1;
        }


        /// <summary>
        /// Casts all rays to find targets
        /// Can use break in a foreach to optimize
        /// </summary>
        public IEnumerable<Transform> FindTargets()
        {
            var halfov = _fov * .5f;
            var adder = _fov / _rays;
            
            //  Looping through all possible rays and shooting a cast

            for (var angle = -halfov + adder * .5f; angle < halfov; angle += adder)
            {
                if(!CastRay(angle, out var hit)) continue;
                yield return hit.transform;
            }
        }


        /// <summary>
        /// Casts a single ray at the given angle
        /// </summary>
        private bool CastRay(float angle, out RaycastHit hitInfo)
        {
            var direction = Utils.Quaternion(y: angle) * transform.forward;
            return Physics.Raycast(transform.position, direction, out hitInfo, _rayLength, _interactMask);
        }


        /// <summary>
        /// Checks if the target is still in range
        /// </summary>
        public bool CanSee(Transform target)
        {
            if (!target.gameObject.activeInHierarchy) return false;
            
            var direction = transform.position - target.position;
            return direction.sqrMagnitude < _rayLength * _rayLength;
        }


        /// <summary>
        /// Draws the utility rays
        /// </summary>
        private void OnDrawGizmos()
        {
            var halfov = _fov * .5f;
            var adder = _fov / _rays;
            var position = transform.position;
            
            //  Looping through all possible rays
            
            for (var i = -halfov + adder*.5f; i < halfov; i += adder)
            {
                var rot = Utils.Quaternion(y: i);
                Gizmos.DrawRay(position, rot * transform.forward * _rayLength);
            }
        }
    }
}