using System.Collections.Generic;
using Game.Scripts.Utils;
using NPC.Brains;
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
        

        protected override void Awake()
        {
            base.Awake();
            
            //  Creating an array with empty lists
            
            _platoons = new List<Platoon>[_teamAmount];
            for (var i = 0; i < _platoons.Length; i++)
                _platoons[i] = new List<Platoon>();
        }


        /**
         * Request a platoon
         */
        public void RequestPlatoon(UnitBrain brain)
        {
            //  Initializing
            
            var brainTransform = brain.transform;
            var team           = brain.team;
            
            //  Assigning platoon to me
            
            var platoon = !brain.platoon ? brain.platoon = new Platoon(team, brain) : brain.platoon;

            //  Finding all units near me

            var colliders = Physics
                .OverlapSphere(brainTransform.position, _platoonSearchRadius, _unitLayerName, QueryTriggerInteraction.Ignore);

            //  Finding out what to do with all platoons
            
            foreach (var collider in colliders)
            {
                if(platoon.Count >= _maxPlatoonSize) break;
                
                //  Getting component
                
                if(!collider.TryGetComponent(out UnitBrain unitBrain)) continue;
                if(unitBrain.team != team) continue;
                
                //  Setting and checking platoon
                
                if(unitBrain.platoon == platoon) continue;

                if (unitBrain.platoon == null)
                    platoon.AddUnit(unitBrain);
                
                else if (platoon.Count + unitBrain.platoon.Count < _maxPlatoonSize)
                    unitBrain.platoon.MigratePlatoon(platoon);
            }
            
            Debug.Log(_platoons[(int)team].Count);
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