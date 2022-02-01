using Controllers;
using UnityEngine;

namespace NPC.Behaviours.Steering
{
    public class PursueSteering : AIBehavior
    {
        private Vector3 _targetPos;
        
        
        public override void PhysicsUpdate()
        {
            var targetPos = GetFutureTargetPos(target);
            movement.AddForce(GetDirection(targetPos - transform.position));
        }

        
        /// <summary>
        /// Get the future position of the target
        /// </summary>
        protected Vector3 GetFutureTargetPos(Transform target)
        {
            var prevTargetPos = _targetPos;
            _targetPos = target.position;

            return _targetPos + (prevTargetPos - _targetPos) / Time.fixedDeltaTime * settings.pursueLookAhead;
        }
    }
}