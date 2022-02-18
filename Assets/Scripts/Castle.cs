using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Controllers;
using UI;

public class Castle : MonoBehaviour
{
    public UnityEvent OnDie;
    public UnityEvent OnGetHit;

    private void Start()
    {
        //Set the value of the healthSlider
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
