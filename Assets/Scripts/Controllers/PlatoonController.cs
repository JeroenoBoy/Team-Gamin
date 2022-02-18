using System;
using System.Collections;
using System.Collections.Generic;
using NPC.Brains;
using NPC.UnitData;
using Platoons;
using UnityEngine;

namespace Controllers
{
    public class PlatoonController : MonoBehaviour
    {
        [SerializeField] private float _startDelay  = .5f;
        [SerializeField] private float _searchDelay = 2f;

        [Header("Debug")]
        [SerializeField] private bool _isMaster = false;
        public  Platoon   platoon;
        
        private UnitBrain _brain;

        
        #region Properties

        public UnitSettings unitSettings => _brain.UnitSettings;
        public UnitTeam team => unitSettings.team;
        private bool isMaster => _isMaster;

        #endregion


        /**
         * Start the coroutine
         */
        private void OnEnable()
        {
            StartCoroutine(SearchLoop());
        }
        
        
        /**
         * The loop for finding platoons
         */
        private IEnumerator SearchLoop()
        {
            yield return new WaitForSeconds(_startDelay);
            PlatoonManager.instance.RequestPlatoon(this);
            
            while (true)
            {
                yield return new WaitForSeconds(_searchDelay);
                if (_isMaster) PlatoonManager.instance.RequestPlatoon(this);
            }
        }


        /**
         * Check if I should leave the platoon
         */
        private void Update()
        {
            if(_isMaster || !platoon) return;
            
            var direction = platoon.Master.transform.position - transform.position;
            var man = PlatoonManager.instance;

            if (direction.sqrMagnitude > man.SearchRadius * man.SearchRadius)
                platoon.RemoveUnit(this);
        }


        /**
         * Initialize the component
         */
        private void Awake()
        {
            platoon = null;
            _brain = GetComponent<UnitBrain>();
        }


        /**
         * Join a platoon
         */
        public void JoinPlatoon(Platoon platoon)
        {
            platoon.AddUnit(this);
        }

        
        /**
         * Leave a platoon
         */
        public void LeavePlatoon()
        {
            platoon.RemoveUnit(this);
        }


        #region Custom Events

        private void OnStateChange()
        {
            if(platoon && unitSettings.state.IsGuardPath()) platoon.RemoveUnit(this);
        }
        

        private void OnPlatoonUpdate()
        {
            _isMaster = platoon.Master == this;
        }

        #endregion
    }
}