using System;
using UnityEditor;
using UnityEngine;
using Util;

// using Util;

namespace Controllers
{
    /// <summary>
    /// Handles the movement of the character
    /// </summary>
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private float _acceleration;
        [SerializeField] private float _weight;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _minVelocity;

        [Header("Rotation")]
        [SerializeField] private float _rotationSpeed;

        public  Vector3 velocity     { get; set; }
        public  Vector3 currentForce { get; set; }
        private Vector3 _oldForce;


        #region Properties
        
        /// <summary>
        /// Get the current max speed of the controller 
        /// </summary>
        public float maxSpeed
        {
            get => _maxSpeed;
            set => _maxSpeed = value;
        }

        /// <summary>
        /// Get the current max speed of the controller 
        /// </summary>
        public float weight
        {
            get => _weight;
            set => _weight = value;
        }

        /// <summary>
        /// Get the current force
        /// </summary>
        public Vector3 lastForce => _oldForce;

        #endregion
        

        /// <summary>
        /// Fixes input is zero
        /// </summary>
        private Quaternion _forward => Quaternion.LookRotation(velocity == Vector3.zero ? transform.forward : velocity);


        //  Unity Messages
        
        
        /// <summary>
        /// Updates the physics
        /// </summary>
        private void FixedUpdate()
        {
            //  Calculating the desired new velocity

            _oldForce = currentForce = Vector3.ClampMagnitude(currentForce, maxSpeed);
            
            var direction  = (currentForce - velocity);
            var desiredVel = direction.normalized * _acceleration / _weight;

            //  Applying velocity
            
            velocity += desiredVel * Time.fixedDeltaTime;
            
            //  Checking if the velocity is lower than a certain value

            var velSqr = _minVelocity * _minVelocity;
            if(currentForce.sqrMagnitude <= velSqr && velocity.sqrMagnitude < velSqr)
                velocity = Vector3.zero;
            
            //  Resetting the force

            currentForce = Vector3.zero;
        }
        

        /// <summary>
        /// Updates the position
        /// </summary>
        private void Update()
        {
            var targetSpeed = _rotationSpeed * Time.deltaTime;
            
            transform.position += velocity * Time.deltaTime;
            transform.rotation  = Quaternion.RotateTowards(transform.rotation, _forward, targetSpeed);
        }

        
        /// <summary>
        /// Draws the current velocity, and the target velocity for debug purposes
        /// </summary>
        private void OnDrawGizmos()
        {
            var pos = transform.position;
            
            DisplayVector.Draw(pos, velocity,  Color.blue);
            DisplayVector.Draw(pos, _oldForce, Color.red);
        }

        
        #region Public functions

        /// <summary>
        /// Add a force to this controller
        /// </summary>
        public void AddForce(Vector3 addForce)
            => currentForce += addForce;

        #endregion
    }
}