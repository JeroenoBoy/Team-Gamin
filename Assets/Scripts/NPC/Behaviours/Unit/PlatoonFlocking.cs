using System.Linq;
using NPC.Behaviours.Avoidance;
using NPC.Brains;
using Platoons;
using UnityEngine;

namespace NPC.Behaviours.Unit
{
    public class PlatoonFlocking : Flocking
    {
        private Platoon _platoon => ((UnitBrain)stateController).Platoon;


        protected override Vector3 CalculateForce(Vector3 center, float sqrDist)
        {
            if(!_platoon) return Vector3.zero;
            
            var units = _platoon.units.Select(t=>t.transform).ToArray();
            var separationTargets = units.Where(t => (t.position - center).sqrMagnitude < sqrDist);
            
            //  Calculating forces

            var targetCohesionForce   = CalculateForce(units,             cohesionDistance,   cohesionForce);
            var targetSeparationForce = CalculateForce(separationTargets, separationDistance, separationForce, true);

            //  Checking if min force is smaller than a certain value else return force
            
            var force = targetCohesionForce - targetSeparationForce;
            return force.sqrMagnitude < minForce
                ? Vector3.zero
                : force;
        }
    }
}