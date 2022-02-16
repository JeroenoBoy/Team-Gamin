using System.Collections.Generic;
using Controllers;
using Game.Scripts.Utils;
using NPC.Brains;
using NPC.UnitData;
using UnityEngine;

namespace Platoons
{
    public class PlatoonManager : Singleton<PlatoonManager>
    {
        [SerializeField] private float           _maxPlatoonSize;
        [SerializeField] private float           _platoonSearchRadius;
        [SerializeField] private int             _teamAmount;
        [SerializeField] private LayerMask       _unitLayerName;
        [SerializeField] private List<Platoon>[] _platoons;
        
        public float searchRadius => _platoonSearchRadius;
        
        
        private void Awake()
        {
            //  Creating an array with empty lists
            
            _platoons = new List<Platoon>[_teamAmount];
            for (var i = 0; i < _platoons.Length; i++)
                _platoons[i] = new List<Platoon>();
        }


        /**
         * Request a platoon
         */
        public void RequestPlatoon(PlatoonController controller)
        {
            if(controller.unitSettings.state.IsGuardPath()) return;
            
            //  Initializing
            
            var brainTransform = controller.transform;
            var team           = controller.team;
            
            //  Assigning platoon to me
            
            var platoon = !controller.platoon ? controller.platoon = new Platoon(team, controller) : controller.platoon;
            if(platoon.Count >= _maxPlatoonSize) return;

            //  Finding all units near me

            var colliders = Physics
                .OverlapSphere(brainTransform.position, _platoonSearchRadius, _unitLayerName, QueryTriggerInteraction.Ignore);

            //  Finding out what to do with all platoons
            
            foreach (var collider in colliders)
            {
                if(platoon.Count >= _maxPlatoonSize) break;
                
                //  Getting component
                
                if(!collider.TryGetComponent(out PlatoonController ctrl)) continue;
                
                //  Validation checks
                
                if(ctrl.team != team) continue;
                if(ctrl.platoon == platoon) continue;
                if(ctrl.unitSettings.state.IsGuardPath()) continue;
    
                //  Adding unit to platoon or migrating platoon

                if (!ctrl.platoon)
                    platoon.AddUnit(ctrl);
                
                else if (platoon.Count + ctrl.platoon.Count < _maxPlatoonSize)
                    ctrl.platoon.MigratePlatoon(platoon);
            }
        }


        /**
         * Removes a platoon from the list
         */
        public void AddPlatoon(Platoon platoon)
        {
            _platoons[(int)platoon.team].Add(platoon);
        }


        /**
         * Removes a platoon from the list
         */
        public void RemovePlatoon(Platoon platoon)
        {
            _platoons[(int)platoon.team].Remove(platoon);
        }
    }
}