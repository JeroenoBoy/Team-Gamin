using System;
using System.Linq;
using Controllers;
using NPC.Utility;
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
        private static readonly int _distanceHash = Animator.StringToHash("Distance");
        private static readonly int _targetHash   = Animator.StringToHash("HasTarget");

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
            UpdateTarget();
        }


        /**
         * Updates the target
         */
        private void UpdateTarget()
        {
            var position = transform.position;
            
            //  Getting the closest unit in the other team

            var closest = _eyes.hits
                .Where  (t => t.transform.TryGetComponent(out UnitBrain _brain) && _brain.team != _team)
                .OrderBy(t => (t.transform.position - position).sqrMagnitude)
                .FirstOrDefault().transform;

            //  Setting new target
            
            if(!TrySetTarget(closest)) return;
            
            //  Updating animator values
            
            animator.SetFloat(_distanceHash, (target.position - position).FastMag());
        }

        #endregion
        
        
        /**
         * Gets run when the health changes
         */
        private void HealthChange()
        {
            animator.SetInteger(_healthHash, _healthComponent.health);
        }


        /**
         * Try to set the target to the closest
         */
        public bool TrySetTarget(Transform closest)
        {
            switch (closest != null)
            {
                case true when target:
                    var closestDistance = (closest.position - transform.position).sqrMagnitude;
                    var currentDistance =  (target.position - transform.position).sqrMagnitude;

                    if (closestDistance > currentDistance) target = closest;
                    return true;
                
                case true when !target:
                    target = closest;
                    return _eyes.CanSee(target);
                
                case false when target:
                    return _eyes.CanSee(target);
                
                default:
                    return false;
            }
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