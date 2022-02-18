using UnityEngine;

namespace NPC.Behaviours.Steering
{
    public class FleeSteering : AIBehavior
    {
        public override void PhysicsUpdate()
        {
            if(!target) return;

            var direction = transform.position - target.position;
            
            //  If the distance is higher than a certain amount, dont run at all

            var distance = direction.sqrMagnitude;
            if(distance > settings.MaxFleeDistance * settings.MaxFleeDistance) return;
            
            //  Exactly like Seek but inversed

            movement.AddForce(GetDirection(direction.normalized * movement.MaxSpeed));
        }
    }
}