using System;
using UnityEngine;
using Util;

namespace NPC.Behaviours.Avoidance
{
    public class AvoidObstacle : PermanentBehavior
    {
        public override void PhysicsUpdate()
        {
            if (!FindObstacle(out var hit)) return;
            
            //  Getting directions & points
            
            var force     = movement.currentForce;
            var position  = transform.position;
            var center    = hit.transform.position;
            var direction = (position - center).With(y: 0);
            
            //  Calculating angle

            var angle = Vector3.SignedAngle(direction, force, Vector3.up);
            var inverseAngle = settings.inverseAvoidAngleOffset - Math.Abs(angle) * Mathf.Sign(angle);
            
            //  Returning force
            
            var rot = Quaternion.Euler(0, inverseAngle,0);
            movement.AddForce(rot * -force * settings.avoidObstacleForce);
        }


        /// <summary>
        /// Check if there is an obstacle nearby
        /// </summary>
        public bool FindObstacle(out RaycastHit hit)
        {
            var position = transform.position;
            var forward = transform.forward;
            
            //  Checking if there is a target in front of the player

            return Physics.Raycast(position, forward, out hit, settings.avoidObstacleDistance, settings.avoidObstacleMask);
        }


        /// <summary>
        /// Draw the box cast
        /// </summary>
        public override void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, transform.forward * settings.avoidObstacleDistance);
        }
    }
}