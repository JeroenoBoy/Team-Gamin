using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPC.UnitData;
using Util;

public class UpgradeArea : Singleton<UpgradeArea>
{
    //makes a dictionary with the settings and a coroutine
    private readonly Dictionary<UnitSettings, Coroutine> _units = new Dictionary<UnitSettings, Coroutine>();

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out UnitSettings unit))
        {
            //makes a coroutine and adds it to a unit
            var coroutine = StartCoroutine(AddAttackDamage(unit));
            _units.Add(unit, coroutine);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out UnitSettings unit))
        {
            //removes the coroutine and removes the units from the dictionary
            var coroutine = _units[unit];
            StopCoroutine(coroutine);
            _units.Remove(unit);
        }
    }

    
    /// <summary>
    /// function to be called when inside upgrade area 
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    private IEnumerator AddAttackDamage(UnitSettings unit)
    {
        yield return new WaitForSeconds(5);

        //increase attack for all units in the trigger
        unit.AttackDamage = 200;
        unit.Upgrade();
    }
}
