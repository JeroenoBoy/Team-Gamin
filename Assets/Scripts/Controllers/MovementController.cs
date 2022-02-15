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

        [Space]
        public bool canMove = true;

        public  Vector3 velocity            { get; set; }
        public  float   angularVelocity     { get; set; }
        public  Vector3 currentForce        { get; set; }
        public  float   currentAngularForce { get; set; }
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


        /// <summary>
        /// Updates the physics
        /// </summary>
        private void FixedUpdate()
        {
            //  calculating the desired new velocity

            _oldForce = currentForce = Vector3.ClampMagnitude(currentForce, maxSpeed);
            
            var direction  = (currentForce - velocity);
            var desiredVel = direction.normalized * _acceleration / _weight;

            //  Applying velocity
            
            velocity += desiredVel * Time.fixedDeltaTime;
            
            //  Checking if the velocity is lower than a certain value

            var velSqr = _minVelocity * _minVelocity;
            if(currentForce.sqrMagnitude <= velSqr && velocity.sqrMagnitude < velSqr)
                velocity = Vector3.zero;
            
            //  Rotating towards

            angularVelocity = Mathf.Clamp(currentAngularForce, -_rotationSpeed, _rotationSpeed);
            
            //  Resetting the force
            
            currentAngularForce = Vector3.SignedAngle(transform.forward, currentForce, transform.up);
            currentForce        = Vector3.zero;
        }


        /// <summary>
        /// Updates the position
        /// </summary>
        private void Update()
        {
            if(!canMove) return;
            
            transform.position    += velocity * Time.deltaTime;
            transform.eulerAngles += new Vector3(0, Mathf.Clamp(angularVelocity, -_rotationSpeed, _rotationSpeed) * Time.deltaTime, 0);
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

        /// <summary>
        /// Add a force to this controller
        /// </summary>
        public void AddAngularVelocity(float addForce)
            => currentAngularForce += addForce;

        #endregion
    }
}