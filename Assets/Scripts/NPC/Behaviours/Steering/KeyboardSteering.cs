using Controllers;
using UnityEngine;

namespace NPC.Behaviours.Steering
{
    public class KeyboardSteering : AIBehavior
    {
        private float _deadzone = 0.1f;


        public override void PhysicsUpdate()
        {
            var input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            movement.AddForce(Vector3.ClampMagnitude(input, 1) * movement.MaxSpeed);
        }
    }
}