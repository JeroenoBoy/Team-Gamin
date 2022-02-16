using System;
using System.Linq;
using NPC.UnitData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI
{
    public class EvilAI : MonoBehaviour
    {
        private newStatPoints _statPoints;
        private SpawnManager  _spawner;
        private BehaviourMenu _behaviourMenu;


        /**
         * Initialize all values
         */
        private void Awake()
        {
            _spawner       = GetComponent<SpawnManager>();
            _statPoints    = gameObject.AddComponent<newStatPoints>();
            _behaviourMenu = gameObject.AddComponent<BehaviourMenu>();

            _statPoints.enabled    = false;
            _behaviourMenu.enabled = false;
            _spawner.statPoints    = _statPoints;
            _spawner.behaviourMenu = _behaviourMenu;

            _statPoints.statPoints = 100f;
            _statPoints.data = new []
            {
                new statpointsdata { value = 0 },
                new statpointsdata { value = 0 },
                new statpointsdata { value = 0 },
                new statpointsdata { value = 0 },
                new statpointsdata { value = 0 },
            };
        }
        
        
        /**
         *  Randomize all data values 
         */
        public void BeforeSpawn()
        {
            var total      = _statPoints.data.Sum(data => data.value = Random.Range(0, 1f));
            var totalMulti = _statPoints.statPoints / total;
            
            foreach (var data in _statPoints.data)
            {
                data.value *= totalMulti;
            }

            RandomizeBehaviour();
        }


        /**
         * Set the behaviour to randomized values
         */
        private void RandomizeBehaviour()
        {
            _behaviourMenu.unitState = UnitStateExtensions.Random();
            _behaviourMenu.pathIndex = Random.Range(0, 3);
        }
    }
}