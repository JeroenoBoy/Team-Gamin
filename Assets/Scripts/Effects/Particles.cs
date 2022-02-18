using UnityEngine;

public class Particles : MonoBehaviour
{
    public Vector3 Speed; 


    void Update()
    {
        transform.Rotate(Speed * Time.deltaTime);
    }
}
