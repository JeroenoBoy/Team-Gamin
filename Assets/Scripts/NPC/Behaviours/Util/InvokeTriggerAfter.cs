using UnityEngine;

namespace NPC.Behaviours.Util
{
    public class InvokeTriggerAfter : AIBehavior
    {
        [SerializeField] private float  _minDelay;
        [SerializeField] private float  _maxDelay;
        [SerializeField] private string _triggerName;

        private int   _triggerHash;
        private float _endAt;
        private bool  _exited;


        /**
         * Getting has because its slightly faster this way
         */
        protected override void Start()
        {
            _triggerHash = Animator.StringToHash(_triggerName);
        }


        /**
         * Resetting the timer
         */
        protected override void Enter()
        {
            _endAt  = Time.time + Random.Range(_minDelay, _maxDelay);
            _exited = false;
            
            animator.ResetTrigger(_triggerHash);
        }


        /**
         * Setting the trigger after the delay ended
         */
        public override void PhysicsUpdate()
        {
            if(_exited || Time.time < _endAt) return;
            _exited = true;
            animator.SetTrigger(_triggerHash);
        }
    }
}