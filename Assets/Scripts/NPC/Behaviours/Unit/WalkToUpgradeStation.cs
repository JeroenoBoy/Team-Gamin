namespace NPC.Behaviours.Unit
{
    public class WalkToUpgradeStation : AIBehavior
    {
        public override void PhysicsUpdate()
        {
            movement.AddForce(MoveWithArrive(UpgradeArea.Instance.transform.position));
        }
    }
}