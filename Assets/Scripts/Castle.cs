using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Controllers;

public class Castle : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnDie;

    [SerializeField]
    private UnityEvent OnGetHit;

    private void Start()
    {
        GetComponentInChildren<Healthbar>().SetMaxHealth(GetComponent<HealthierController>().maxHealth);
    }

    private void OnDamage()
    {
        OnGetHit?.Invoke();
    }

    private void OnDeath()
    {
        OnDie?.Invoke();
    }
}
