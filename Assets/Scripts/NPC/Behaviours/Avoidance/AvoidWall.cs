using System.Linq;
using NPC.Brains;
using UnityEngine;
using Util;

namespace NPC.Behaviours.Avoidance
{
    public class AvoidWall : AvoidObstacle
    {
        [SerializeField] private string _avoidLayer;

        private int _layer;
        
        public Transform[] targets => ((UnitBrain)stateController).targets;


        protected override void Start()
        {
            _layer = LayerMask.GetMask(_avoidLayer);
        }


        public override void PhysicsUpdate()
        {
            var obstacles = targets
                .Where(t => t.transform.HasLayer(_layer))
                .ToArray();
            
            var addForce = movement.maxSpeed * settings.avoidWallForce;
            
            //  Looping thru all obstacles

            foreach (var obstacle in obstacles)
            {
                var direction = transform.position - obstacle.position;
                var distance    = direction.FastMag() - obstacle.lossyScale.FastMag();
                
                //  Calculating target force
             
                var percentageDistance = Mathf.Clamp(distance / settings.avoidObstacleDistance, 0, 1);
                movement.AddForce(direction.normalized * percentageDistance * addForce);
            }
        }
    }
}