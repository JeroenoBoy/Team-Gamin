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
        }


        public override void PhysicsUpdate()
        {
            if(_died || Time.time < _dieAt) return;
            
            _died = true;
            stateController.gameObject.SetActive(false);
        }
    }
}