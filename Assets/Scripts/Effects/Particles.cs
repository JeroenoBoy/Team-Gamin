using UnityEngine;

public class Particles : MonoBehaviour
{
    public Vector3 speed; 


    void Update()
    {
        transform.Rotate(speed * Time.deltaTime);
    }
}
