using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Castle : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnDie;
    
    private void OnDeath()
    {
        OnDie?.Invoke();
    }
}
