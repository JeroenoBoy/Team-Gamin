using System;
using UnityEngine;

namespace NPC.Behaviours.Util
{
    public class SetBoolAtState : AIBehavior
    {
        [SerializeField] private string _boolName;
        
        
        protected override void Enter()
        {
            animator.SetBool(_boolName, true);
        }


        protected override void Exit()
        {
            animator.SetBool(_boolName, false);
        }
    }
}