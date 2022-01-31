using UnityEngine;
using Util;

namespace NPC.Behaviours.Steering
{
    public class SeekSteering : AIBehavior
    {
        [SerializeField] private float _speedMulti = 1f;
        
        public override void PhysicsUpdate()
        {
            if(!target) return;
            
            var targetPos = target.position;
            var myPos     = transform.position;
            
            //  Calculating the distance

            var direction = (targetPos - myPos).With(y: 0);
            movement.AddForce(GetDirection(direction * _speedMulti));
        }
    }
}