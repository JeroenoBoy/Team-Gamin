using UnityEngine;

namespace NPC.Behaviours.Steering
{
    public class EvadeSteering : PursueSteering
    {
        public override void PhysicsUpdate()
        {
            var position    = transform.position;
            var targetPos   = target.position;
            
            //  Checking if im far enough away from my target
            
            var direction = position - targetPos;
            var distance    = direction.sqrMagnitude;

            if (distance > settings.MaxFleeDistance * settings.MaxFleeDistance) return;
            
            //  Applying the force
            
            var estimatePos = GetFutureTargetPos(target);
            movement.AddForce(GetDirection((position - estimatePos).normalized * movement.MaxSpeed));
        }
    }
}