using NPC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;

public class BlockBehaviour : AIBehavior
{
    protected override void Start()
    {
        base.Start();
        
        var health = GetComponent<HealthController>();
        health.isBlocking = true;
    }
}
