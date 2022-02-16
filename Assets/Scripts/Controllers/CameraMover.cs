//credit to this vid https://www.youtube.com/watch?v=Lz74CCrxzjs
using UnityEngine;
using Util;

public class CameraMover : MonoBehaviour
{
    private Camera cam;
    public Vector3 clamp;

    [Header("Focus Object")]
    [SerializeField, Tooltip("Enable double-click to focus on objects?")]
    private bool doFocus = false;
    [SerializeField] private float focusLimit = 100f;
    [SerializeField] private float minFocusDistance = 5.0f;
    private float doubleClickTime = .15f;
    private float cooldown = 0;
    [Header("Undo - Only undoes the Focus Object - The keys must be pressed in order.")]
    [SerializeField] private KeyCode firstUndoKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode secondUndoKey = KeyCode.Z;

    [Header("Speed Values")]
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float rotateSpeed = 3.0f;
    [SerializeField] private float zoomSpeed = 15.0f;
    [SerializeField] private float moveDamp = 0.3f;

    //Cache last pos and rot be able to undo last focus object action.
    private Quaternion prevRot = new Quaternion();
    private Vector3 prevPos = new Vector3();

    [Header("Movement Keycodes")]
    [SerializeField] private KeyCode forwardKey = KeyCode.W;
    [SerializeField] private KeyCode backKey    = KeyCode.S;
    [SerializeField] private KeyCode leftKey    = KeyCode.A;
    [SerializeField] private KeyCode rightKey   = KeyCode.D;
    [SerializeField] private KeyCode upKey      = KeyCode.Space;
    [SerializeField] private KeyCode downKey    = KeyCode.LeftShift;

    [Header("Anchored Keycodes")]
    [SerializeField] private KeyCode anchoredMoveKey = KeyCode.Mouse2;
    [SerializeField] private KeyCode anchoredRotateKey = KeyCode.Mouse1;


    public Vector3 up => Vector3.up;
    public Vector3 forward => transform.forward.With(y: 0).normalized;
    public Vector3 right => transform.right.With(y: 0).normalized;
    public Vector3 down => Vector3.down;
    public Vector3 backward => -forward;
    public Vector3 left => -right;

    private Vector3 _vel; // Used for smoothing
    
    private Rigidbody _rb;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        
        SavePosAndRot();
        cam = Camera.main;
    }

    private void Update()
    {
        Focus();
        
        float _mouseMoveY = Input.GetAxis("Mouse Y");
        float _mouseMoveX = Input.GetAxis("Mouse X");

        //rotate the camera when anchored
        
        if (Input.GetKey(anchoredRotateKey))
        {
            transform.RotateAround(transform.position, transform.right, _mouseMoveY * -rotateSpeed);
            transform.RotateAround(transform.position, Vector3.up, _mouseMoveX * rotateSpeed);
        }

        //  Getting the Speed

        Vector3 targetVel = CalculateTargetSpeed(_mouseMoveX, _mouseMoveY);
        _rb.velocity = Vector3.SmoothDamp(_rb.velocity, targetVel, ref _vel, moveDamp, moveSpeed);


        //clamping

        if (cam.transform.position.y >= clamp.y)
        {
            cam.transform.position = cam.transform.position.With(y: clamp.y);
        }
        if (cam.transform.position.x >= clamp.x || cam.transform.position.x <= -clamp.x)
        {
            cam.transform.position = cam.transform.position.With(x: clamp.x * Mathf.Sign(cam.transform.position.x));
        }
        if (cam.transform.position.z >= clamp.z || cam.transform.position.z <= -clamp.z)
        {
            cam.transform.position = cam.transform.position.With(z: clamp.z * Mathf.Sign(cam.transform.position.z));
        }
    }


    private void Focus()
    {
        if (!doFocus)
            return;

        //double click for focussing on a target with a collider
        if (cooldown > 0 && Input.GetKeyDown(KeyCode.Mouse0))
            FocusObject();

        if (Input.GetKeyDown(KeyCode.Mouse0))
            cooldown = doubleClickTime;

        if (Input.GetKey(firstUndoKey))
        {
            if (Input.GetKey(secondUndoKey))
                GoBackToLastPosition();
        }

        cooldown -= Time.deltaTime;
    }


    private Vector3 CalculateTargetSpeed(float _mouseMoveX, float _mouseMoveY)
    {
        Vector3 targetSpeed = Vector3.zero;

        //move and rotate the camera

        if (Input.GetKey(forwardKey)) targetSpeed +=  forward * moveSpeed;
        if (Input.GetKey(backKey))    targetSpeed += backward * moveSpeed;
        if (Input.GetKey(rightKey))   targetSpeed +=    right * moveSpeed;
        if (Input.GetKey(leftKey))    targetSpeed +=     left * moveSpeed;
        if (Input.GetKey(upKey))      targetSpeed +=       up * moveSpeed;
        if (Input.GetKey(downKey))    targetSpeed +=     down * moveSpeed;

        //move the camera when anchored
        
        if (Input.GetKey(anchoredMoveKey))
        {
            targetSpeed +=    up * _mouseMoveY * -moveSpeed;
            targetSpeed += right * _mouseMoveX * -moveSpeed;
        }

        return targetSpeed;
    }
    

    private void FocusObject()
    {
        //to be able to undo
        SavePosAndRot();

        //if we are looking at an object in the scene go to its position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, focusLimit))
        {
            GameObject target = hit.collider.gameObject;
            Vector3 targetPos = target.transform.position;
            Vector3 targetSize = hit.collider.bounds.size;

            transform.position = targetPos + GetOffSet(targetPos, targetSize);

            transform.LookAt(target.transform);
        }
    }

    private void SavePosAndRot()
    {
        prevRot = transform.rotation;
        prevPos = transform.position;
    }

    private void GoBackToLastPosition()
    {
        transform.position = prevPos;
        transform.rotation = prevRot;
    }

    private Vector3 GetOffSet(Vector3 targetPos, Vector3 targetSize)
    {
        Vector3 dirTotarget = targetPos - transform.position;

        float focusDistance = Mathf.Max(targetSize.x, targetSize.z);
        focusDistance = Mathf.Clamp(focusDistance, minFocusDistance, focusDistance);

        return -dirTotarget.normalized * focusDistance;
    }
}
