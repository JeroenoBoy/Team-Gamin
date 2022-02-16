using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;
using Game.Scripts.Utils;

public class HealingFountain : Singleton<HealingFountain>
{
    [SerializeField] private int   _healAmount;
    [SerializeField] private float _healDelay;
    
    private List<HealthController> Agents = new List<HealthController>();

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(Heal());
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (!coll.TryGetComponent(out HealthController agent)) return;
     
        Agents.Add(agent);
    }

    private void OnTriggerExit(Collider coll)
    {
        if (!coll.TryGetComponent(out HealthController agent)) return;
        
        Agents.Remove(agent);
    }

    IEnumerator Heal()
    {
        while (true)
        {
            yield return new WaitForSeconds(_healDelay);
            foreach (var healthController in Agents)
                healthController.Heal(_healAmount);
        }
    }
}
