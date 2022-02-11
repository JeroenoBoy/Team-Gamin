namespace NPC.Behaviours.Unit
{
    public class WalkToHealFountain : AIBehavior
    {
        public override void PhysicsUpdate()
        {
            var position = HealingFountain.instance.transform.position;
            movement.AddForce(MoveWithArrive(position));
        }
    }
}