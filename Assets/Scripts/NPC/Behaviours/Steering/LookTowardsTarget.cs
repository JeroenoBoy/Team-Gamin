using UnityEngine;

namespace NPC.Behaviours.Steering
{
    public class LookTowardsTarget : AIBehavior
    {
        public override void PhysicsUpdate()
        {
            if(!target) return;
            movement.currentAngularForce = Vector3.SignedAngle(transform.forward, target.position - transform.position, transform.up);
        }
    }
}