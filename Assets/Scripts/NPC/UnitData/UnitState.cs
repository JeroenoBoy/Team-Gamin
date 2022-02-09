namespace NPC.UnitData
{
    public enum UnitState
    {
        Aggressive,
        Defensive,
        Loyal,
        Wander,
        GuardPathA,
        GuardPathB
    }



    public static class UnitStateExtensions
    {
        public static bool IsGuardPath(this UnitState self)
        {
            return self == UnitState.GuardPathA || self == UnitState.GuardPathB;
        }
    }
}