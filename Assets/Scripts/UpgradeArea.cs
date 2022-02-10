using System.Collections;
using UnityEngine;
using NPC.UnitData;

public class UpgradeArea : MonoBehaviour
{
    private UnitSettings settings => FindObjectOfType<UnitSettings>();
    [SerializeField] private bool isInside = false;

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Unit")) yield break;
        isInside = true;
        while (isInside)
        {
            //increases attack damage
            settings.attackDamage += 5;

            //caps attack damage
            if (settings.attackDamage >= 30)
                settings.attackDamage = 30;

            //waits 5 seconds
            yield return new WaitForSeconds(5);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isInside = false;
    }
}
