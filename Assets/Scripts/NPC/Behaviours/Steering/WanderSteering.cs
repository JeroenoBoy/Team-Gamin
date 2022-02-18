using UnityEditor;
using UnityEngine;

namespace NPC.Behaviours.Steering
{
    public class WanderSteering : AIBehavior
    {
        [SerializeField] private float _forceMultiplier = 1f;
        
        private float _pointAngle = 0f;
        
        
        public override void PhysicsUpdate()
        {
            //  Updating the point angle
            
            _pointAngle += Random.Range(-settings.WanderNoiseAngle, settings.WanderNoiseAngle) * Time.fixedDeltaTime;
            
            //  Setting targetVel to the point

            var forward       = transform.forward;
            var distance      = settings.WanderCircleDistance * forward;
            var rotation      = Quaternion.Euler(0,_pointAngle,0) * forward;
            var relativePoint = rotation * settings.WanderCircleRadius;

            movement.AddForce((relativePoint + distance) * _forceMultiplier);
        }



#if UNITY_EDITOR
        public override void OnDrawGizmos()
        {
            var currentPos = transform.position;
            var forward    = transform.forward;
            
            var centerPos  = currentPos + forward * settings.WanderCircleDistance;
            var pointPos   = Quaternion.Euler(0,_pointAngle,0) * forward * settings.WanderCircleRadius + centerPos;
            
            //  Drawing the distance line
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(currentPos, centerPos);
            
            //  Drawing the wander disc
            
            Handles.color = Color.green;
            Handles.DrawWireDisc(centerPos, Vector3.up, settings.WanderCircleRadius);
            
            //  Drawing the target point
            
            Handles.DrawSolidDisc(pointPos, Vector3.up, 0.3f);
        }
#endif
    }
}