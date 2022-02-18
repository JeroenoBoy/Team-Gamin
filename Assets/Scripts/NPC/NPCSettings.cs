using UnityEngine;

namespace NPC
{
    [CreateAssetMenu(fileName = "NPCSettings", menuName = "NPCSettings", order = 0)]
    public class NPCSettings : ScriptableObject
    {
        public float minSeekDistance;
        public float maxFleeDistance;
        public float pursueLookAhead;
        
        [Header("Path Finding")]
        public float minNodeDistance;

        [Header("Arrive Settings")]
        public float stopDistance;
        public float slowDistance;

        [Header("Wander Setting")]
        public float wanderCircleDistance;
        public float wanderCircleRadius;
        public float wanderNoiseAngle;

        [Header("Avoid Object")]
        public float     avoidObstacleForce;
        public float     avoidObstacleDistance;
        public float     inverseAvoidAngleOffset;
        public LayerMask avoidObstacleMask;

        [Header("Hiding")]
        public float     hideMaxDistance;
        public float     hideLookInterval;
        public float     hideTargetDistance;
        public LayerMask hideMask;

        [Header("Flocking")]
        public float     flockCohesionDistance;
        public float     flockCohesionMaxForce;
        public float     flockSeparationDistance;
        public float     flockSeparationMaxForce;
        public LayerMask flockMask;

        [Header("Castle Settings")]
        public float castleDistance;
    }
}