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
            var _p = platoon;
            var units = platoon.units.Select(t=>t.transform).ToArray();
            
            
            //  Filtering targets
            
            var center = transform.position;
            var sqrDist = settings.flockSeparationDistance * settings.flockSeparationDistance;
            
            var separationTargets
                = units.Where(t => (t.transform.position - center).sqrMagnitude < sqrDist);
            
            //  Calculating forces

            var cohesionForce  = CalculateForce(units,             settings.flockCohesionDistance, settings.flockCohesionMaxForce);
            var separationForce = CalculateForce(separationTargets, settings.flockCohesionDistance, settings.flockCohesionMaxForce);

            //  Checking if min force is smaller than a certain value else return force
            
            var force = cohesionForce + separationForce;
            
            movement.AddForce(force.sqrMagnitude < minForce * minForce
                ? Vector3.zero
                : force);
        }
    }
}