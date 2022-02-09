

namespace NPC.Behaviours.Unit {
    public class ForgetTarget : AIBehavior
    {
        public override void PhysicsUpdate()
        {
            stateController.target = null;
        }
    }
}