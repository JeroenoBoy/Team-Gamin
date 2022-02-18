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
            if(_eyes.Hits == null) return;
            
            var obstacles = _eyes.Hits
                .Where(t => t.transform && t.transform.HasLayer(_layer))
                .ToArray();
            
            var addForce = movement.maxSpeed * settings.AvoidObstacleForce;
            
            //  Looping thru all obstacles

            foreach (var hit in obstacles)
            {
                var distance = (transform.position - hit.point).magnitude;
                if(distance > settings.AvoidObstacleDistance) continue;
                
                //  Calculating target force
             
                var percentageDistance = distance / settings.AvoidObstacleDistance;
                movement.AddForce(hit.normal.With(y: 0) * percentageDistance * addForce);
            }
        }


#if UNITY_EDITOR
        /**
         * Draw fancy gizmos
         */
        public override void OnDrawGizmos()
        {
            if(_eyes.Hits == null) return;
            
            foreach (var hit in _eyes.Hits.Where(t => t.transform.HasLayer(_layer) && (t.point - transform.position).sqrMagnitude < settings.AvoidObstacleDistance*settings.AvoidObstacleDistance))
            {
                Gizmos.color = Handles.color = Color.magenta;
                
                Handles.DrawSolidDisc(hit.point, hit.normal, 0.5f);
                Gizmos.DrawLine(hit.point, hit.point + hit.normal);
            }
        }
#endif
    }
}