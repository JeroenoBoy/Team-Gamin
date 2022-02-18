using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;
using Util;

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
        //Add agent from the list
        if (!coll.TryGetComponent(out HealthController agent)) return;
     
        Agents.Add(agent);
    }

    private void OnTriggerExit(Collider coll)
    {
        //Remove agent from the list
        if (!coll.TryGetComponent(out HealthController agent)) return;
        
        Agents.Remove(agent);
    }

    /// <summary>
    /// Heal every agent in the AgentsArray every time the _healDelay is over
    /// </summary>
    /// <returns></returns>
    IEnumerator Heal()
    {
        while (true)
        {
            yield return new WaitForSeconds(_healDelay);

            foreach (var healthController in Agents) //Heal every agent
                healthController.Heal(_healAmount);
        }
    }
}
