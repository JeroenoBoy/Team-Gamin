using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;

public class HealingFountain : MonoBehaviour
{
    public List<HealthController> agents = new List<HealthController>();

    private void Start() => StartCoroutine(Heal());

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.TryGetComponent(out HealthController agent))
        {
            StartCoroutine(Heal());
            agents.Add(agent);
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.TryGetComponent(out HealthController agent))
            agents.Remove(agent);
    }

    IEnumerator Heal()
    {
        while (agents.Count > 0)
        {
            yield return new WaitForSeconds(1);

            for (int i = 0; i < agents.Count; i++)
            {
                agents[i].Heal(1);
            }
        }
    }
}
