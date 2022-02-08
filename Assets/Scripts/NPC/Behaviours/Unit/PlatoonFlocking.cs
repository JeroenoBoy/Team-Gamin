using System.Linq;
using NPC.Behaviours.Avoidance;
using NPC.Brains;
using Platoons;
using UnityEngine;

namespace NPC.Behaviours.Unit
{
    public class PlatoonFlocking : Flocking
    {
        public Platoon platoon => ((UnitBrain)stateController).platoon;
        
        
        public override void PhysicsUpdate()
        {
            if(!platoon) return;
            var units = platoon.units.Select(t=>t.transform).ToArray();
            
            //  Filtering targets
            
            var center = transform.position;
            var sqrDist = separationForce * separationForce;

            var separationTargets = units.Where(t => (t.position - center).sqrMagnitude < sqrDist);
            
            //  Calculating forces

            var targetCohesionForce   = CalculateForce(units,             cohesionDistance,   cohesionForce);
            var targetSeparationForce = CalculateForce(separationTargets, separationDistance, separationForce, true);

            //  Checking if min force is smaller than a certain value else return force
            
            var force = targetCohesionForce - targetSeparationForce;
            
            movement.AddForce(force.sqrMagnitude < minForce
                ? Vector3.zero
                : force);
        }
    }
}