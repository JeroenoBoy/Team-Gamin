using Controllers;
using UnityEngine;

namespace NPC.Behaviours.Avoidance
{
    public class AvoidWall : PermanentBehavior
    {
        public override void PhysicsUpdate()
        {
            if (!FindObstacle(out var hit)) return;
            
            //  Calculating distance

            var percentageDistance = hit.distance / settings.avoidObstacleDistance;
            
            //  Returning force

            movement.AddForce(hit.normal * percentageDistance * movement.maxSpeed * settings.avoidWallForce);
        }


        /// <summary>
        /// Check if there is an obstacle nearby
        /// </summary>
        public bool FindObstacle(out RaycastHit hit)
        {
            var position = transform.position;
            var forward = transform.forward;
            
            //  Checking if there is a target in front of the player

            return Physics.Raycast(position, forward, out hit, settings.avoidWallDistance, settings.avoidWallMask);
        }


        /// <summary>
        /// Draw the box cast
        /// </summary>
        public override void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, transform.forward * settings.avoidWallDistance);
        }
    }
}