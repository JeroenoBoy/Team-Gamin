using System.Linq;
using NPC.UnitData;
using UnityEngine;

namespace AI
{
    public static class RandomizeAIExtensions
    {
        /**
         * Randomize all the inputs in the values
         */
        public static void Randomize(this EvilAI ai)
        {
            var total = ai.statPoints.data.Sum(data => data.value = Random.Range(0, 1f));
            var totalMulti = ai.statPoints.statPoints / total;
            
            foreach (var data in ai.statPoints.data)
            {
                data.value *= totalMulti;
            }

            RandomizeBehaviour(ai);
        }



        private static void RandomizeBehaviour(this EvilAI ai)
        {
            ai.behaviourMenu.unitState = UnitStateExtensions.Random();
        }


        /**
         * Create the required target
         */
        public static void InitData(this EvilAI ai)
        {
            ai.statPoints.data = new []
            {
                new statpointsdata { value = 0 },
                new statpointsdata { value = 0 },
                new statpointsdata { value = 0 },
                new statpointsdata { value = 0 },
                new statpointsdata { value = 0 },
            };
        }
    }
}