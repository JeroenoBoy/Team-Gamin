//credit to this vid https://www.youtube.com/watch?v=Lz74CCrxzjs

using UnityEngine;
using Util;

namespace Controllers
{
    public class CameraMover : MonoBehaviour
    {
        [SerializeField] private Vector3 _clamp;
        
        [Header("Undo - Only undoes the Focus Object - The keys must be pressed in order.")]
        [SerializeField] private KeyCode _firstUndoKey = KeyCode.LeftControl;
        [SerializeField] private KeyCode _secondUndoKey = KeyCode.Z;

        [Header("Clamping angles")]
        [SerializeField] private float _maxAngle =  80f;
        [SerializeField] private float _minAngle = -70f; 

        [Header("Speed Values")]
        [SerializeField] private float _moveSpeed = 20f;
        [SerializeField] private float _rotateSpeed = 3f;
        [SerializeField] private float _moveDamp = .08f;

        [Header("Movement Keycodes")]
        [SerializeField] private KeyCode _forwardKey = KeyCode.W;
        [SerializeField] private KeyCode _backKey    = KeyCode.S;
        [SerializeField] private KeyCode _leftKey    = KeyCode.A;
        [SerializeField] private KeyCode _rightKey   = KeyCode.D;
        [SerializeField] private KeyCode _keyCode      = KeyCode.Space;
        [SerializeField] private KeyCode _downKey    = KeyCode.LeftShift;

        [Header("Anchored Keycodes")]
        [SerializeField] private KeyCode _anchoredMoveKey = KeyCode.Mouse2;
        [SerializeField] private KeyCode _anchoredRotateKey = KeyCode.Mouse1;

        private Vector3   _vel; // Used for smoothing
        private Rigidbody _rb;
        private Camera    _cam;
    
    
        #region Properties
    
        public Vector3 Up => Vector3.up;
        public Vector3 Forward => transform.forward.With(y: 0).normalized;
        public Vector3 Right => transform.right.With(y: 0).normalized;
        public Vector3 Down => Vector3.down;
        public Vector3 Backward => -Forward;
        public Vector3 Left => -Right;

        #endregion
    
    
        /**
     * Initialize the scripts
     */
        private void Start()
        {
            _rb  = GetComponent<Rigidbody>();
            _cam = Camera.main;
        }

    
        /**
     * Update the camera's position
     */
        private void Update()
        {
            float mouseMoveY = Input.GetAxis("Mouse Y");
            float mouseMoveX = Input.GetAxis("Mouse X");

            //  Rotate the camera when anchored
        
            if (Input.GetKey(_anchoredRotateKey))
            {
                transform.RotateAround(transform.position, transform.right, mouseMoveY * -_rotateSpeed);
                transform.RotateAround(transform.position, Vector3.up, mouseMoveX * _rotateSpeed);

                Vector3 angles = transform.eulerAngles;
                float   angle  = (angles.x + 180) % 360 - 180;
                transform.eulerAngles = angles.With(x: Mathf.Clamp(angle, _minAngle, _maxAngle), z: 0);
            }

            //  Getting the Speed

            Vector3 targetVel = CalculateTargetSpeed(mouseMoveX, mouseMoveY);
            _rb.velocity = Vector3.SmoothDamp(_rb.velocity, targetVel, ref _vel, _moveDamp, _moveSpeed);

            //clamping

            if (_cam.transform.position.y >= _clamp.y)
                _cam.transform.position = _cam.transform.position.With(y: _clamp.y);
        
            if (_cam.transform.position.x >= _clamp.x || _cam.transform.position.x <= -_clamp.x)
                _cam.transform.position = _cam.transform.position.With(x: _clamp.x * Mathf.Sign(_cam.transform.position.x));
        
            if (_cam.transform.position.z >= _clamp.z || _cam.transform.position.z <= -_clamp.z)
                _cam.transform.position = _cam.transform.position.With(z: _clamp.z * Mathf.Sign(_cam.transform.position.z));
        }


        /**
     * Calculates the speed the camera wants to go at
     */
        private Vector3 CalculateTargetSpeed(float mouseMoveX, float mouseMoveY)
        {
            Vector3 targetSpeed = Vector3.zero;

            //move and rotate the camera

            if (Input.GetKey(_forwardKey)) targetSpeed +=  Forward * _moveSpeed;
            if (Input.GetKey(_backKey))    targetSpeed += Backward * _moveSpeed;
            if (Input.GetKey(_rightKey))   targetSpeed +=    Right * _moveSpeed;
            if (Input.GetKey(_leftKey))    targetSpeed +=     Left * _moveSpeed;
            if (Input.GetKey(_keyCode))      targetSpeed +=       Up * _moveSpeed;
            if (Input.GetKey(_downKey))    targetSpeed +=     Down * _moveSpeed;

            //move the camera when anchored
        
            if (Input.GetKey(_anchoredMoveKey))
            {
                targetSpeed +=    Up * mouseMoveY * -_moveSpeed;
                targetSpeed += Right * mouseMoveX * -_moveSpeed;
            }

            return targetSpeed;
        }
    }
}
