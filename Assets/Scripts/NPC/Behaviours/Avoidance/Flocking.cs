using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;

namespace NPC.Behaviours.Avoidance
{
    public delegate Vector3 CalculatorFunction(Transform target);
    
    
    public class Flocking : PermanentBehavior
    {
        protected const float minForce = 0.1f * 0.1f;

        [SerializeField] private float _multiplier = 1f;

        [Header("Override Settings")]
        [SerializeField] private bool  _override           = false;
        [SerializeField] private float _cohesionDistance   = 4f;
        [SerializeField] private float _separationDistance = 2f;
        [SerializeField] private float _cohesionForce      = 5f;
        [SerializeField] private float _separationForce    = 5f;
        [SerializeField] private LayerMask _layerMask;


        protected float cohesionForce      => _override ? _cohesionForce      : settings.flockCohesionMaxForce;
        protected float cohesionDistance   => _override ? _cohesionDistance   : settings.flockCohesionDistance;
        protected float separationForce    => _override ? _separationForce    : settings.flockSeparationMaxForce;
        protected float separationDistance => _override ? _separationDistance : settings.flockSeparationDistance;
        protected LayerMask flockMask      => _override ? _layerMask : settings.flockMask;
        
        
        
        public override void PhysicsUpdate()
        {
            var targets = FindTargets(cohesionDistance);
            
            //  Filtering targets

            var center = transform.position;
            var sqrDist  = separationDistance * separationDistance;
            
            var separationTargets
                = targets.Where(t => (t.position - center).sqrMagnitude < sqrDist);
            
            //  Calculating forces

            var targetCohesionForce   = CalculateForce(targets,           cohesionDistance,   cohesionForce);
            var targetSeparationForce = CalculateForce(separationTargets, separationDistance, separationForce, true);

            //  Checking if min force is smaller than a certain value else return force
            
            var force = (targetCohesionForce - targetSeparationForce) * _multiplier;
            
            movement.AddForce(force.sqrMagnitude < minForce
                ? Vector3.zero
                : force);
        }


        protected Vector3 CalculateForce(IEnumerable<Transform> targets, float distance, float maxForce, bool inverse = false)
        {
            var transform = this.transform;
            var center = transform.position;

            //  Calculate the force based on distance

            var calculator = CalculateFunction(center, distance, maxForce, inverse);
            var vector = targets.Aggregate(Vector3.zero, (vector, transform) => vector + calculator(transform));
            
            //  Returning the force

            return Vector3.ClampMagnitude(vector, Mathf.Abs(maxForce));
        }


        private CalculatorFunction CalculateFunction(Vector3 center, float distance, float maxForce, bool inverse = false)
        {
            return inverse switch
            {
                true => t =>
                {
                    if (t == transform) return Vector3.zero;

                    var direction = (t.position - center).With(y: 0);

                    var vecDistance = direction.magnitude;
                    var percentageForce = vecDistance / distance;

                    return (1 - percentageForce) * maxForce * direction.normalized;
                },

                false => (t) =>
                {
                    if (t == transform) return Vector3.zero;

                    var direction = (t.position - center).With(y: 0);

                    var vecDistance = direction.magnitude;
                    var percentageForce = Mathf.Clamp(vecDistance / distance, 0, 1);

                    return percentageForce * maxForce * direction.normalized;
                }
            };
        }


        /// <summary>
        /// Find targets near the player
        /// </summary>
        private Transform[] FindTargets(float distance)
        {
            return Physics.OverlapSphere(transform.position, distance, flockMask)
                .Select(c => c.transform)
                .ToArray();
        }
    }
}