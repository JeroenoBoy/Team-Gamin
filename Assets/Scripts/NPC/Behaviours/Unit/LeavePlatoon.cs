using NPC.Utility;

namespace NPC.Behaviours.Unit
{
    public class LeavePlatoon : UnitBehaviour
    {
        public override void PhysicsUpdate()
        {
            if (platoon) platoon.RemoveUnit(unitBrain);
        }
    }
}