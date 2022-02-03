using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Castle : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnDie;

    [SerializeField]
    private UnityEvent OnGetHit;

    private void OnDamage()
    {
        OnGetHit?.Invoke();
    }

    private void OnDeath()
    {
        OnDie?.Invoke();
    }
}
