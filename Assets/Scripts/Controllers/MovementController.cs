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
        public bool CanMove = true;

        public Vector3 Velocity { get; set; }
        public float AngularVelocity { get; set; }
        public Vector3 CurrentForce { get; set; }
        public float currentAngularForce { get; set; }
        private Vector3 _oldForce;


        #region Properties

        /// <summary>
        /// Get the current max speed of the controller 
        /// </summary>
        public float MaxSpeed
        {
            get => _maxSpeed;
            set => _maxSpeed = value;
        }

        /// <summary>
        /// Get the current max speed of the controller 
        /// </summary>
        public float Weight
        {
            get => _weight;
            set => _weight = value;
        }

        #endregion


        /// <summary>
        /// Updates the physics
        /// </summary>
        private void FixedUpdate()
        {
            //  calculating the desired new velocity

            _oldForce = CurrentForce = Vector3.ClampMagnitude(CurrentForce, MaxSpeed);

            var direction = CurrentForce - Velocity;
            var desiredVel = direction.normalized * _acceleration / _weight;

            //  Applying velocity

            Velocity += desiredVel * Time.fixedDeltaTime;

            //  Checking if the velocity is lower than a certain value

            var velSqr = _minVelocity * _minVelocity;
            if (CurrentForce.sqrMagnitude <= velSqr && Velocity.sqrMagnitude < velSqr)
                Velocity = Vector3.zero;

            //  Rotating towards

            AngularVelocity = Mathf.Clamp(currentAngularForce, -_rotationSpeed, _rotationSpeed);

            //  Resetting the force

            currentAngularForce = Vector3.SignedAngle(transform.forward, CurrentForce, transform.up);
            CurrentForce = Vector3.zero;
        }


        /// <summary>
        /// Updates the position
        /// </summary>
        private void Update()
        {
            if (!CanMove) return;

            transform.position += Velocity * Time.deltaTime;
            transform.eulerAngles += new Vector3(0, Mathf.Clamp(AngularVelocity, -_rotationSpeed, _rotationSpeed) * Time.deltaTime, 0);
        }


        /// <summary>
        /// Draws the current velocity, and the target velocity for debug purposes
        /// </summary>
        private void OnDrawGizmos()
        {
            var pos = transform.position;

            DisplayVector.Draw(pos, Velocity, Color.blue);
            DisplayVector.Draw(pos, _oldForce, Color.red);
        }


        #region Public functions

        /// <summary>
        /// Add a force to this controller
        /// </summary>
        public void AddForce(Vector3 addForce) => CurrentForce += addForce;

        /// <summary>
        /// Add a force to this controller
        /// </summary>
        public void AddAngularVelocity(float addForce) => currentAngularForce += addForce;

        #endregion
    }
}