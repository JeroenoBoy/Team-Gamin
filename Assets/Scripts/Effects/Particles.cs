using UnityEngine;

namespace Effects
{
    public class Particles : MonoBehaviour
    {
        public Vector3 Speed; 
    
        private void Update()
        {
            transform.Rotate(Speed * Time.deltaTime);
        }
    }
}
