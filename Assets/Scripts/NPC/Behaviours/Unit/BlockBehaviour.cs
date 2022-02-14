using NPC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;

public class BlockBehaviour : AIBehavior
{
    HealthController health;
    protected override void Start()
    {
        base.Start();

        health = GetComponent<HealthController>();
        health.isBlocking = true;
    }

    protected override void Exit()
    {
        base.Exit();

        health.isBlocking = false;
    }
}
