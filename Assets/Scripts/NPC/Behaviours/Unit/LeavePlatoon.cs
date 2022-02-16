using Controllers;
using NPC.Utility;

namespace NPC.Behaviours.Unit
{
    public class LeavePlatoon : UnitBehaviour
    {
        private PlatoonController _platoonController;
        
        
        protected override void Start()
        {
            _platoonController = GetComponent<PlatoonController>();
        }
        

        public override void PhysicsUpdate()
        {
            if (platoon) platoon.RemoveUnit(_platoonController);
        }
    }
}