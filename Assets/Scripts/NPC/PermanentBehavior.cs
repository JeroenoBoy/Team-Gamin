using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace NPC
{
    public abstract class PermanentBehavior : AIBehavior
    {
        [SerializeField] private bool _unload = false;


        public override void OnStateEnter(Animator anim, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_unload)
                base.OnStateEnter(anim, stateInfo, layerIndex);
            else if (!started)
                base.OnStateEnter(anim, stateInfo, layerIndex);
        }
        
        
        public override void OnStateExit(Animator anim, AnimatorStateInfo stateInfo, int stateMachinePathHash)
        {
            if(_unload) base.OnStateExit(anim, stateInfo, stateMachinePathHash);
        }
    }
}