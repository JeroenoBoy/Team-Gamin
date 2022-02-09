using System;
using System.Collections.Generic;

namespace NPC.UnitData
{
    public enum UnitState
    {
        Aggressive,
        Defensive,
        Loyal,
        Wander,
        GuardPathA,
        GuardPathB,
    }



    public static class UnitStateExtensions
    {
        private static readonly UnitState[] _states = (UnitState[])Enum.GetValues(typeof(UnitState));
        
        
        public static bool IsGuardPath(this UnitState self)
        {
            return self == UnitState.GuardPathA || self == UnitState.GuardPathB;
        }


        public static UnitState Random()
        {
            return _states[UnityEngine.Random.Range(0, _states.Length)];
        }
    }
}