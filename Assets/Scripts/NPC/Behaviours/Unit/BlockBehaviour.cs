using Controllers;
using NPC.Utility;

namespace NPC.Behaviours.Unit
{
    public class BlockBehaviour : UnitBehaviour
    {
        protected override void Enter()
        {
            healthController.IsBlocking = true;
        }

        protected override void Exit()
        {
            healthController.IsBlocking = false;
        }
    }
}
