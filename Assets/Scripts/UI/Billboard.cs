using UnityEngine;

namespace UI
{
    public class Billboard : MonoBehaviour
    {
        public Transform cam;

        private void Start()
        {
            //gets camera reference
            cam = Camera.main.transform;
        }

        void LateUpdate()
        {
            //rotates ui object towards camera
            transform.LookAt(transform.position + cam.forward);
        }
    }
}
