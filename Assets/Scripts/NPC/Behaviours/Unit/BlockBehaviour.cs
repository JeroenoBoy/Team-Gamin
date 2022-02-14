using NPC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;

public class BlockBehaviour : AIBehavior
{
    private HealthController _health;
    protected override void Start()
    {
        base.Start();

        _health = GetComponent<HealthController>();
        _health.isBlocking = true;
    }

    protected override void Exit()
    {
        base.Exit();

        _health.isBlocking = false;
    }
}
