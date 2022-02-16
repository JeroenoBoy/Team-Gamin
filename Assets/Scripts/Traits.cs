using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPC.UnitData;

public class Traits : MonoBehaviour
{
    private UnitSettings settings;

    public string CurrentTrait;

    private void Start()
    {
        settings = GetComponent<UnitSettings>();

        int random = Random.Range(0, 5);

        if (random == 1)
        {
            //strong trait

            CurrentTrait = "Strong";

            settings.attackDamage += 10;
            settings.attackSpeed += 5;
        }
        if (random == 2)
        {
            //speedy trait

            CurrentTrait = "Speedy";

            settings.movementSpeed += 25;
        }
        if(random == 3)
        {
            //tanky trait

            CurrentTrait = "Tanky";

            settings.defense += 25;
        }
        if(random == 4)
        {
            //marksman trait

            CurrentTrait = "Marksman";

            settings.sightRange += 25;
        }
    }
}
