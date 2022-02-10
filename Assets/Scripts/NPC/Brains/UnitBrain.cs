using System;
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
        private static readonly int _healthHash     = Animator.StringToHash("Health");
        private static readonly int _stateHash      = Animator.StringToHash("State");
        private static readonly int _distanceHash   = Animator.StringToHash("Distance");
        private static readonly int _platoonHash    = Animator.StringToHash("PlatoonSize");
        private static readonly int _targetHash     = Animator.StringToHash("HasTarget");
        private static readonly int _nearCastleHash = Animator.StringToHash("NearCastle");
        private static readonly int _isDeadHash     = Animator.StringToHash("IsDead");
        private static readonly int _diedHash       = Animator.StringToHash("Died");

        public Transform castleTarget;

        private HealthController _healthComponent;
        private Eyes             _eyes;

        private bool _hasTarget;
        
        public Transform[]  targets { get; private set; }
        public Platoon      platoon;
        public UnitSettings unitSettings { get; private set; }


        private void OnEnable()
        {
            Reset();
        }


        public void Bind()
        {
            movementController.maxSpeed = unitSettings.movementSpeed;
            _eyes.rayLength             = (int)unitSettings.sightRange;
            _healthComponent.maxHealth  = (int)unitSettings.defense;
            _healthComponent.health     = _healthComponent.maxHealth;
            _healthComponent.baseHealth = unitSettings.baseDefence;
        }


        /**
         * Initiate the script
         */
        protected override void Awake()
        {
            base.Awake();
            _healthComponent = GetComponent<HealthController>();
            _eyes            = GetComponent<Eyes>();
            unitSettings      = GetComponent<UnitSettings>();
        }


        /**
         * Mainly to assign platoons
         */
        private IEnumerator Start()
        {
            while (true)
            {
                PlatoonManager.instance.RequestPlatoon(this);
                yield return new WaitForSeconds(2);
            }
        }


        /**
         * Remove the unit from its platoon
         */
        private void OnDisable()
        {
            platoon?.RemoveUnit(this);
        }


        #region Update

        
        /**
         * Manage the updates
         */
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            UpdateCastleInRange();
            if(_eyes.hits != null) UpdateTarget();
        }


        private void UpdateCastleInRange()
        {
            animator.SetBool(_nearCastleHash, (castleTarget.position - transform.position).sqrMagnitude < settings.castleDistance * settings.castleDistance);
            
            if(!_hasTarget)
                animator.SetFloat(_distanceHash, (castleTarget.position - transform.position).magnitude);
        }


        /**
         * Updates the target
         */
        private void UpdateTarget()
        {
            var position = transform.position;
            
            //  Getting the closest unit in the other team

            var closest = _eyes.hits
                .Where  (t => t.transform.TryGetComponent(out UnitBrain brain) && brain.team != team)
                .OrderBy(t => (t.transform.position - position).sqrMagnitude)
                .FirstOrDefault().transform;

            //  Setting new target
            
            if(!TrySetTarget(closest)) return;
            
            //  Updating animator values
            
            animator.SetFloat(_distanceHash, (target.position - position).magnitude);
        }

        #endregion
        
        
        #region Custom Events
        
        
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
            animator.SetBool(_isDeadHash, true);
        }


        /**
         * Gets run when the UnitState changes
         */
        private void OnStateChange()
        {
            if(platoon && unitSettings.state.IsGuardPath()) platoon.RemoveUnit(this);
            animator.SetInteger(_stateHash, (int)unitSettings.state);
        }


        private void OnPlatoonUpdate()
        {
            animator.SetInteger(_platoonHash, platoon.Count);
        }
        
        #endregion


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
            get => unitSettings.team;
            set => unitSettings.team = value;
        }
        

        public override Transform target
        {
            get
            {
                //  To avoid some 
                if (!base.target && _hasTarget)
                    target = null;
                if (base.target && !base.target.gameObject.activeInHierarchy)
                    target = null;
                
                
                return base.target ? base.target : castleTarget;
            }
            set
            {
                //  More efficient on the animator
                switch (_hasTarget)
                {
                    case true when !value:
                        animator.SetBool(_targetHash, _hasTarget = false);
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