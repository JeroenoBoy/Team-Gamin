using UnityEngine;
using UnityEngine.Serialization;

namespace NPC
{
    [CreateAssetMenu(fileName = "NPCSettings", menuName = "NPCSettings", order = 0)]
    public class NPCSettings : ScriptableObject
    {
         public float MinSeekDistance;
         public float MaxFleeDistance;
         public float PursueLookAhead;
        
         [Header("Path Finding")]
         public float minNodeDistance;

         [Header("Arrive Settings")]
         public float StopDistance;
         public float SlowDistance;

         [Header("Wander Setting")]
         public float WanderCircleDistance;
         public float WanderCircleRadius;
         public float WanderNoiseAngle;

         [Header("Avoid Object")]
         public float     AvoidObstacleForce;
         public float     AvoidObstacleDistance;
         public float     InverseAvoidAngleOffset;
         public LayerMask AvoidObstacleMask;

         [Header("Hiding")]
         public float     HideMaxDistance;
         public float     HideLookInterval;
         public float     HideTargetDistance;
         public LayerMask HideMask;

         [Header("Flocking")]
         public float     FlockCohesionDistance;
         public float     FlockCohesionMaxForce;
         public float     FlockSeparationDistance;
         public float     FlockSeparationMaxForce;
         public LayerMask FlockMask;

        [Header("Castle Settings")]
        public float castleDistance;
    }
}