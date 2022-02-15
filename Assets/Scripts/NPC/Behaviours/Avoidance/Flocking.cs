using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace NPC.Behaviours.Avoidance
{
    public delegate Vector3 CalculatorFunction(Transform target);
    
    
    public class Flocking : PermanentBehavior
    {
        protected const float minForce = 0.1f * 0.1f;

        [SerializeField] private float _multiplier          = 1f;
        [SerializeField] private float _updateTime          = 1f;
        [SerializeField] private float _calculateForceDelay = 0.2f;
        
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

        private float       _nextUpdate;
        private Transform[] _targets;
        private Vector3     _force;
        
        
        protected override void Enter()
        {
            StartCoroutine(CalculateForce());
        }


        /**
         * Calculate the force
         */
        private IEnumerator CalculateForce()
        {
            yield return new WaitForSeconds(Random.Range(0f, _calculateForceDelay));
            
            while (true)
            {
                //  Some values
    
                var center = transform.position;
                var sqrDist  = separationDistance * separationDistance;

                _force = CalculateForce(center, sqrDist);
                
                yield return new WaitForSeconds(_calculateForceDelay);
            }
        }

        protected virtual Vector3 CalculateForce(Vector3 center, float sqrDist)
        {
            var targets = FindTargets(cohesionDistance);
                
            var separationTargets
                = targets.Where(t => (t.position - center).sqrMagnitude < sqrDist);
                
            //  Calculating forces
    
            var targetCohesionForce   = CalculateForce(targets,           cohesionDistance,   cohesionForce);
            var targetSeparationForce = CalculateForce(separationTargets, separationDistance, separationForce, true);
    
            //  Checking if min force is smaller than a certain value else return force
                
            var force = (targetCohesionForce - targetSeparationForce) * _multiplier;

            return force.sqrMagnitude < minForce
                ? Vector3.zero
                : force;
        }


        /**
         * Applies the force
         */
        public override void PhysicsUpdate()
        {
            movement.AddForce(_force);
        }


        /**
         * Calculate the force
         */
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


        /**
         * Get the wanted calculation function
         */
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


        /**
         * Find the targets near the player
         */
        private Transform[] FindTargets(float distance)
        {
            if (_nextUpdate > Time.time) return _targets;
            _nextUpdate += _updateTime;
            
            return _targets = Physics.OverlapSphere(transform.position, distance, flockMask)
                .Select(c => c.transform)
                .ToArray();
        }
    }
}