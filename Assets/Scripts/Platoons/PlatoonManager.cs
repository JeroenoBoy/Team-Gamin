using System.Collections.Generic;
using Controllers;
using NPC.Brains;
using NPC.UnitData;
using UnityEngine;
using Util;

namespace Platoons
{
    public class PlatoonManager : Singleton<PlatoonManager>
    {
        [SerializeField] private float           _maxPlatoonSize;
        [SerializeField] private float           _platoonSearchRadius;
        [SerializeField] private int             _teamAmount;
        [SerializeField] private LayerMask       _unitLayerName;
        [SerializeField] private List<Platoon>[] _platoons;
        
        public float SearchRadius => _platoonSearchRadius;
        
        
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
            if(controller.UnitSettings.state.IsGuardPath()) return;
            
            //  Initializing
            
            var brainTransform = controller.transform;
            var team           = controller.Team;
            
            //  Assigning platoon to me
            
            var platoon = !controller.Platoon ? controller.Platoon = new Platoon(team, controller) : controller.Platoon;
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
                
                if(ctrl.Team != team) continue;
                if(ctrl.Platoon == platoon) continue;
                if(ctrl.UnitSettings.state.IsGuardPath()) continue;
    
                //  Adding unit to platoon or migrating platoon

                if (!ctrl.Platoon)
                    platoon.AddUnit(ctrl);
                
                else if (platoon.Count + ctrl.Platoon.Count < _maxPlatoonSize)
                    ctrl.Platoon.MigratePlatoon(platoon);
            }
        }


        /**
         * Removes a platoon from the list
         */
        public void AddPlatoon(Platoon platoon)
        {
            _platoons[(int)platoon.Team].Add(platoon);
        }


        /**
         * Removes a platoon from the list
         */
        public void RemovePlatoon(Platoon platoon)
        {
            _platoons[(int)platoon.Team].Remove(platoon);
        }
    }
}