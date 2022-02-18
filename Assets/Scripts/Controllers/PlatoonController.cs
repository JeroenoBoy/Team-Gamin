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
        
        public  Platoon   Platoon;
        
        private bool      _isMaster = false;
        private UnitBrain _brain;

        
        #region Properties

        public UnitSettings UnitSettings => _brain.UnitSettings;
        public UnitTeam Team => UnitSettings.team;

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
            if(_isMaster || !Platoon) return;
            
            var direction = Platoon.Master.transform.position - transform.position;
            var man = PlatoonManager.instance;

            if (direction.sqrMagnitude > man.SearchRadius * man.SearchRadius)
                Platoon.RemoveUnit(this);
        }


        /**
         * Initialize the component
         */
        private void Awake()
        {
            Platoon = null;
            _brain = GetComponent<UnitBrain>();
        }


        #region Custom Events

        private void OnStateChange()
        {
            if(Platoon && UnitSettings.state.IsGuardPath()) Platoon.RemoveUnit(this);
        }
        

        private void OnPlatoonUpdate()
        {
            _isMaster = Platoon.Master == this;
        }

        #endregion
    }
}