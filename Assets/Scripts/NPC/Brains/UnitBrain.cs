using System;
using System.Linq;
using Controllers;
using UnityEngine;
using Util;

namespace NPC.Brains
{
    public enum UnitState
    {
        Aggressive,
        Defensive,
        Loyal,
        Wander,
        GuardPath
    }
    
    public enum Team
    {
        Red, Blue
    }
    
    
    public class UnitBrain : StateController
    {
        private static readonly int _healthHash   = Animator.StringToHash("Health");
        private static readonly int _stateHash    = Animator.StringToHash("State");
        private static readonly int _targetHash   = Animator.StringToHash("HasTarget");
        private static readonly int _distanceHash = Animator.StringToHash("Distance");

        [SerializeField] private Team      _team;
        [SerializeField] private UnitState _state;

        private HealthController _healthComponent;
        private Eyes             _eyes;

        private bool _hasTarget;


        /**
         * Initiate the script
         */
        protected override void Awake()
        {
            base.Awake();
            _healthComponent = GetComponent<HealthController>();
            _eyes            = GetComponent<Eyes>();

            //  Might look weird yes but its easier this way
            state = _state;
        }


        #region Update

        
        /**
         * Manage the updates
         */
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            targets = _eyes.FindTargets().Distinct().ToArray();
            
            UpdateTarget();
        }


        /**
         * Updates the target
         */
        private void UpdateTarget()
        {
            var position = transform.position;
            
            //  Getting the closest unit in the other team
            
            var closest = targets
                .Where  (t => t.TryGetComponent(out UnitBrain _brain) && _brain.team != _team)
                .OrderBy(t => (t.position - position).sqrMagnitude)
                .First();

            //  Setting new target
            
            if (!closest) { target = null; return; }
            if (target != closest) target = closest;
            
            //  Updating animator values
            
            animator.SetFloat(_distanceHash, (closest.position - position).FastMag());
        }

        #endregion
        
        
        /**
         * Gets run when the health changes
         */
        private void HealthChange()
        {
            animator.SetInteger(_healthHash, _healthComponent.health);
        }



        #region properties

        public UnitState state
        {
            get => _state;
            set => animator.SetInteger(_stateHash, (int)(_state = value));
        }

        
        public Team team
        {
            get => _team;
            set => _team = value;
        }
     
        
        public override Transform target
        {
            get
            {
                //  To avoid some 
                if (!base.target && _hasTarget) target = null;
                return base.target;
            }
            set
            {
                //  More efficient on the animator
                switch (_hasTarget)
                {
                    case true when !value:
                        animator.SetBool(_targetHash, _hasTarget = false);
                        animator.SetFloat(_distanceHash, Mathf.Infinity);
                        break;
                    
                    case false when value:
                        animator.SetBool(_targetHash, _hasTarget = true);
                        break;
                }

                base.target = value;
            }
        }


        public Transform[] targets { get; private set; }

        #endregion
    }
}