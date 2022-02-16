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

    //Cache last pos and rot be able to undo last focus object action.
    private Quaternion prevRot = new Quaternion();
    private Vector3 prevPos = new Vector3();

    [Header("Movement Keycodes")]
    [SerializeField] private KeyCode forwardKey = KeyCode.W;
    [SerializeField] private KeyCode backKey = KeyCode.S;
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    [SerializeField] private KeyCode rightKey = KeyCode.D;

    [Header("Flat Move"), Tooltip("Instead of going where the camera is pointed, the camera moves only on the horizontal plane (Assuming you are working in 3D with default preferences).")]
    [SerializeField] private KeyCode flatMoveKey = KeyCode.LeftShift;

    [Header("Anchored Keycodes")]
    [SerializeField] private KeyCode anchoredMoveKey = KeyCode.Mouse2;
    [SerializeField] private KeyCode anchoredRotateKey = KeyCode.Mouse1;

    private void Start()
    {
        SavePosAndRot();
        cam = Camera.main;
    }

    private void Update()
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

    private void LateUpdate()
    {
        Vector3 move = Vector3.zero;

        //move and rotate the camera

        if (Input.GetKey(forwardKey))
            move += Vector3.forward * moveSpeed;

        if (Input.GetKey(backKey))
            move += Vector3.back * moveSpeed;

        if (Input.GetKey(leftKey))
            move += Vector3.left * moveSpeed;

        if (Input.GetKey(rightKey))
            move += Vector3.right * moveSpeed;

        if (Input.GetKey(flatMoveKey))
        {
            float origY = transform.position.y;

            transform.Translate(move);
            transform.position = new Vector3(transform.position.x, origY, transform.position.z);

            return;
        }

        float _mouseMoveY = Input.GetAxis("Mouse Y");
        float _mouseMoveX = Input.GetAxis("Mouse X");

        //move the camera when anchored
        if (Input.GetKey(anchoredMoveKey))
        {
            move += Vector3.up * _mouseMoveY * -moveSpeed;
            move += Vector3.right * _mouseMoveX * -moveSpeed;
        }

        //rotate the camera when anchored
        if (Input.GetKey(anchoredRotateKey))
        {
            transform.RotateAround(transform.position, transform.right, _mouseMoveY * -rotateSpeed);
            transform.RotateAround(transform.position, Vector3.up, _mouseMoveX * rotateSpeed);
        }

        transform.Translate(move);

        //scroll to zoom
        float _mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(Vector3.forward * _mouseScroll * zoomSpeed);


        //clamping
        if (cam.transform.position.y <= 10)
        {
            cam.transform.position = cam.transform.position.With(y: 10);
        }
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
