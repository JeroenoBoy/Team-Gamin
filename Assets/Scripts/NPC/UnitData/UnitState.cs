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
        public static readonly UnitState[] states = (UnitState[])Enum.GetValues(typeof(UnitState));


        public static bool IsGuardPath(this UnitState self)
        {
            return self == UnitState.GuardPathA || self == UnitState.GuardPathB;
        }


        public static UnitState Random()
        {
            return states[UnityEngine.Random.Range(0, states.Length)];
        }
    }
}