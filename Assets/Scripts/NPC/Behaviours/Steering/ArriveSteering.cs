using UnityEngine;

namespace NPC.Behaviours.Steering
{
    public class ArriveSteering : AIBehavior
    {
        public override void PhysicsUpdate()
        {
            if(!target) return;
            
            //  Move towards the target position with arrival

            movement.AddForce(MoveWithArrive(target.position));
        }
    }
}