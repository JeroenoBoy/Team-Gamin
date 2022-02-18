namespace NPC.Behaviours.Unit
{
    public class WalkToHealFountain : AIBehavior
    {
        public override void PhysicsUpdate()
        {
            var position = HealingFountain.Instance.transform.position;
            movement.AddForce(MoveWithArrive(position));
        }
    }
}