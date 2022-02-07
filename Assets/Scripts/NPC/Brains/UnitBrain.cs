using System.Collections;
using System.Linq;
using Controllers;
using NPC.UnitData;
using NPC.Utility;
using Platoons;
using UnityEngine;

namespace NPC.Brains
{
    public class UnitBrain : StateController
    {
        private static readonly int _healthHash   = Animator.StringToHash("Health");
        private static readonly int _stateHash    = Animator.StringToHash("State");
        private static readonly int _distanceHash = Animator.StringToHash("Distance");
        private static readonly int _platoonHash  = Animator.StringToHash("PlatoonSize");
        private static readonly int _targetHash   = Animator.StringToHash("HasTarget");
        private static readonly int _diedHash     = Animator.StringToHash("Died");

        public Transform castleTarget;
        
        private UnitSettings     _unitSettings;
        private HealthController _healthComponent;
        private Eyes             _eyes;

        private bool _hasTarget;
        
        public Transform[] targets { get; private set; }
        public Platoon     platoon;
        

        /**
         * Initiate the script
         */
        protected override void Awake()
        {
            base.Awake();
            _healthComponent = GetComponent<HealthController>();
            _eyes            = GetComponent<Eyes>();
            _unitSettings    = GetComponent<UnitSettings>();

            //  Setting the options
            
            movementController.maxSpeed = _unitSettings.movementSpeed;
            _eyes.rayLength             = _unitSettings.sightRange;
            _healthComponent.maxHealth  = _unitSettings.baseHealth + _unitSettings.defense;
            _healthComponent.health     = _healthComponent.maxHealth;
        }


        /**
         * Mainly to assign platoons
         */
        private IEnumerator Start()
        {
            while (true)
            {
                PlatoonManager.instance.RequestPlatoon(this);
                animator.SetInteger(_platoonHash, platoon.Count);
                
                yield return new WaitForSeconds(2);
            }
        }


        #region Update

        
        /**
         * Manage the updates
         */
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if(_eyes.hits != null) UpdateTarget();
        }


        /**
         * Updates the target
         */
        private void UpdateTarget()
        {
            var position = transform.position;
            
            //  Getting the closest unit in the other team

            var closest = _eyes.hits
                .Where  (t => t.transform.TryGetComponent(out UnitBrain _brain) && _brain.team != team)
                .OrderBy(t => (t.transform.position - position).sqrMagnitude)
                .FirstOrDefault().transform;

            //  Setting new target
            
            if(!TrySetTarget(closest)) return;
            
            //  Updating animator values
            
            animator.SetFloat(_distanceHash, (target.position - position).magnitude);
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
         * Gets run when the health changes
         */
        private void OnDeath()
        {
            animator.SetTrigger(_diedHash);
        }


        /**
         * Gets run when the UnitState changes
         */
        private void OnStateChange()
        {
            animator.SetInteger(_stateHash, (int)_unitSettings.state);
        }


        #region Helper functions

        /**
         * Try to set the target to the closest
         */
        public bool TrySetTarget(Transform closest)
        {
            var position = transform.position;
            
            switch (closest != null)
            {
                case true when target:
                    var closestDistance = (closest.position - position).sqrMagnitude;
                    var currentDistance =  (target.position - position).sqrMagnitude;

                    if (closestDistance < currentDistance) target = closest;
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

        #endregion



        #region properties


        public UnitTeam team
        {
            get => _unitSettings.team;
            set => _unitSettings.team = value;
        }
        

        public override Transform target
        {
            get
            {
                //  To avoid some 
                if (!base.target && _hasTarget) target = null;
                return base.target ? base.target : castleTarget;
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

        #endregion
    }
}