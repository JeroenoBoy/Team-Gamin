using UnityEngine;

namespace NPC
{
    public abstract class PermanentBehavior : AIBehavior
    {
        [SerializeField] private bool unload = false;
        
        
        public override void OnStateEnter(Animator anim, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (unload)
                base.OnStateEnter(anim, stateInfo, layerIndex);
            else if (!started)
                base.OnStateEnter(anim, stateInfo, layerIndex);
        }
        
        
        public override void OnStateExit(Animator anim, AnimatorStateInfo stateInfo, int stateMachinePathHash)
        {
            if(unload) base.OnStateExit(anim, stateInfo, stateMachinePathHash);
        }
    }
}