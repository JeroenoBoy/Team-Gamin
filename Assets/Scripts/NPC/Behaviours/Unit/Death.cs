using UnityEngine;

namespace NPC.Behaviours.Unit
{
    public class Death : AIBehavior
    {
        [SerializeField] private float _dieAfter;

        private float _dieAt;
        private bool  _died;
        

        protected override void Enter()
        {
            _dieAt = Time.time + _dieAfter;
            _died = false;
            movement.canMove = false;

            if (Physics.Raycast(transform.position, -transform.up, out var raycast, 5f))
                transform.position = raycast.point + Vector3.up * .5f;
        }


        protected override void Exit()
        {
            movement.canMove = true;
        }


        public override void PhysicsUpdate()
        {
            if(_died || Time.time < _dieAt) return;
            
            _died = true;
            stateController.gameObject.SetActive(false);
        }
    }
}