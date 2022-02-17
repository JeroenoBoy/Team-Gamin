using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPC.UnitData;
using Game.Scripts.Utils;
public class UpgradeArea : Singleton<UpgradeArea>
{
    //makes a dictionary with the settings and a coroutine
    public Dictionary<UnitSettings, Coroutine> units = new Dictionary<UnitSettings, Coroutine>();
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out UnitSettings unit))
        {
            //makes a coroutine and adds it to a unit
            var coroutine = StartCoroutine(AddAttackDamage(unit));
            units.Add(unit, coroutine);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out UnitSettings unit))
        {
            //removes the coroutine and removes the units from the dictionary
            var coroutine = units[unit];
            StopCoroutine(coroutine);
            units.Remove(unit);
        }
    }

    /// <summary>
    /// function to be called when inside upgrade area 
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    IEnumerator AddAttackDamage(UnitSettings unit)
    {
        yield return new WaitForSeconds(5);

        //increase attack for all units in the trigger
        unit.attackDamage = 200;
    }
}
