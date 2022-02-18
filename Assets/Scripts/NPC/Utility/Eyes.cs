using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Util;

namespace NPC.Utility
{
    public class Eyes : MonoBehaviour
    {
        [SerializeField] private float     _fov       = 70;
        [SerializeField] private int       _rays      = 30;
        [SerializeField] private int       _minSight  = 3;
        [SerializeField] private int       _rayLength = 30;
        [SerializeField] private LayerMask _interactMask;
        
        public RaycastHit[] Hits { get; private set; }

        public int RayLength
        {
            get => _rayLength;
            set => _rayLength = value;
        }
        
        
        /**
         * Simple checks to avoid infinite loops 
         */
        private void OnValidate()
        {
            if (_fov       < 1) _fov = 1;
            if (_rays      < 1) _rays = 1;
            if (_rayLength < 0) _rayLength = 1;
        }


        
        /**
         * Find all the last targets
         */
        private void FixedUpdate()
        {
            Hits = FindTargets().ToArray();
        }


        /**
         * Casts all rays to find targets
         * Can use break in a foreach to optimize
         */
        public IEnumerable<RaycastHit> FindTargets()
        {
            var halfov = _fov * .5f;
            var adder = _fov / _rays;
            
            //  Looping through all possible rays and shooting a cast

            for (var angle = -halfov + adder * .5f; angle < halfov; angle += adder)
            {
                if(!CastRay(angle, out var hit)) continue;
                yield return hit;
            }
            
            //  Looping thru all the objects in min sight
            var position = transform.position;
            var ray = new Ray(position, Vector3.zero);
            
            //  Looping thru all the enemies nearby 
            foreach (var collider in Physics.OverlapSphere(position, _minSight, _interactMask))
            {
                if (collider.transform == transform) continue;
                
                //  Getting the raycastHit
                
                ray.direction = (collider.transform.position - position).normalized;
                if (ray.direction == Vector3.zero) continue;
                
                if(collider.Raycast(ray, out var hit, _minSight))
                    yield return hit;
            }
        }


        /**
         * Casts a single ray at the given angle
         */
        private bool CastRay(float angle, out RaycastHit hitInfo)
        {
            var direction = Utils.Quaternion(y: angle) * transform.forward;
            return Physics.Raycast(transform.position, direction, out hitInfo, _rayLength, _interactMask);
        }


        /**
         * Checks if the target is still in range
         */
        public bool CanSee(Transform target)
        {
            if (!target || !target.gameObject.activeInHierarchy) return false;
            
            var direction = transform.position - target.position;
            return direction.sqrMagnitude < _rayLength * _rayLength;
        }


#if UNITY_EDITOR
        /**
         * Draws the utility rays
         */
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
            
            Handles.DrawWireDisc(transform.position, transform.up, _minSight);
        }
#endif
    }
}