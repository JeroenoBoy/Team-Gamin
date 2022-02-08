using System;
using System.Linq;
using NPC.Brains;
using NPC.Utility;
using UnityEditor;
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
            if(_eyes.hits == null) return;
            
            var obstacles = _eyes.hits
                .Where(t => t.transform.HasLayer(_layer))
                .ToArray();
            
            var addForce = movement.maxSpeed * settings.avoidObstacleForce;
            
            //  Looping thru all obstacles

            foreach (var hit in obstacles)
            {
                var distance = (transform.position - hit.point).magnitude;
                if(distance > settings.avoidObstacleDistance) continue;
                
                //  Calculating target force
             
                var percentageDistance = distance / settings.avoidObstacleDistance;
                movement.AddForce(hit.normal.With(y: 0) * percentageDistance * addForce);
            }
        }


        /**
         * Draw fancy gizmos
         */
        public override void OnDrawGizmos()
        {
            if(_eyes.hits == null) return;
            
            foreach (var hit in _eyes.hits.Where(t => t.transform.HasLayer(_layer) && (t.point - transform.position).sqrMagnitude < settings.avoidObstacleDistance*settings.avoidObstacleDistance))
            {
                Gizmos.color = Handles.color = Color.magenta;
                
                Handles.DrawSolidDisc(hit.point, hit.normal, 0.5f);
                Gizmos.DrawLine(hit.point, hit.point + hit.normal);
            }
        }
    }
}