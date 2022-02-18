using UnityEngine;

namespace UI
{
    public class Billboard : MonoBehaviour
    {
        private Transform _cam;

        private void Start()
        {
            //gets camera reference
            _cam = Camera.main.transform;
        }

        private void LateUpdate()
        {
            //rotates ui object towards camera
            transform.LookAt(transform.position + _cam.forward);
        }
    }
}
