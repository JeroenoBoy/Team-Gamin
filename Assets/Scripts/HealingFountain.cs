using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;
using Game.Scripts.Utils;

public class HealingFountain : Singleton<HealingFountain>
{
    public List<HealthController> Agents = new List<HealthController>();

    private void Start() => StartCoroutine(Heal());

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.TryGetComponent(out HealthController agent))
        {
            StartCoroutine(Heal());
            Agents.Add(agent);
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.TryGetComponent(out HealthController agent))
            Agents.Remove(agent);
    }

    IEnumerator Heal()
    {
        while (Agents.Count > 0)
        {
            yield return new WaitForSeconds(1);

            for (int i = 0; i < Agents.Count; i++)
            {
                Agents[i].Heal(1);
            }
        }
    }
}
