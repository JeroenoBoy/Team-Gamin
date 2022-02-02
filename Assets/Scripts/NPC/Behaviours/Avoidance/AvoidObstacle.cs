using System;
using System.Linq;
using NPC.Brains;
using NPC.Utility;
using UnityEngine;
using Util;

namespace NPC.Behaviours.Avoidance
{
    public class AvoidObstacle : PermanentBehavior
    {
        [SerializeField] private string _avoidLayer;

        private int  _layer;
        private Eyes _eyes;
        

        /**
         * Initialize the behaviour
         */
        protected override void Start()
        {
            _layer = LayerMask.GetMask(_avoidLayer);
            _eyes  = GetComponent<Eyes>();
        }
        
        
        /**
         * Run the updates
         */
        public override void PhysicsUpdate()
        {
            var obstacles = _eyes.hits
                .Where(t => t.transform.HasLayer(_layer))
                .ToArray();
            
            var addForce = movement.maxSpeed * settings.avoidObstacleForce;
            
            //  Looping thru all obstacles

            foreach (var hit in obstacles)
            {
                var distance = (transform.position - hit.point).FastMag();
                if(distance > settings.avoidObstacleDistance) continue;
                
                //  Calculating target force
             
                var percentageDistance = distance / settings.avoidObstacleDistance;
                movement.AddForce(hit.normal.With(y: 0) * percentageDistance * addForce);
            }
        }


        /// <summary>
        /// Check if there is an obstacle nearby
        /// </summary>
        public bool FindObstacle(out RaycastHit hit)
        {
            var position = transform.position;
            var forward = transform.forward;
            
            //  Checking if there is a target in front of the player

            return Physics.Raycast(position, forward, out hit, settings.avoidObstacleDistance, settings.avoidObstacleMask);
        }
    }
}