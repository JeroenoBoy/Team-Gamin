using Controllers;
using NPC.UnitData;
using UnityEngine;

namespace NPC.Behaviours.Unit
{
    public class AttackBehaviour : AIBehavior
    {
        private UnitSettings _settings;
        
        private float _nextStrike;


        protected override void Start()
        {
            _settings = GetComponent<UnitSettings>();
        }

        
        protected override void Enter()
        {
            animator.speed = _settings.attackSpeed;
            _nextStrike    = Time.time + 1/_settings.attackSpeed;
        }
        

        public override void PhysicsUpdate()
        {
            if(Time.time < _nextStrike) return;
            _nextStrike += 1/_settings.attackSpeed;

            //  Actually dealing the damage
            
            if (target && target.TryGetComponent(out HealthController health))
                health.Damage(_settings.attackDamage);
        }


        protected override void Exit()
        {
            animator.speed = 1;
        }
    }
}