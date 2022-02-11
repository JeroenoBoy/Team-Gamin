using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPC.UnitData;
using Game.Scripts.Utils;
public class UpgradeArea : Singleton<UpgradeArea>
{
    public Dictionary<UnitSettings, Coroutine> units = new Dictionary<UnitSettings, Coroutine>();
    [SerializeField] private bool isInside = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out UnitSettings unit))
        {
            var coroutine = StartCoroutine(AddAttackDamage(unit));
            units.Add(unit, coroutine);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out UnitSettings unit))
        {
            var coroutine = units[unit];
            StopCoroutine(coroutine);
            units.Remove(unit);
        }
    }

    IEnumerator AddAttackDamage(UnitSettings unit)
    {
        yield return new WaitForSeconds(5);

        //increase attack for all units in the trigger
        unit.attackDamage = 200;
    }
}
