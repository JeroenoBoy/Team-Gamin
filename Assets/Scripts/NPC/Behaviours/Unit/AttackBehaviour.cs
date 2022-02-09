using Controllers;
using NPC.UnitData;
using UnityEngine;
using Util;

namespace NPC.Behaviours.Unit
{
    public class AttackBehaviour : AIBehavior
    {
        [SerializeField] private LayerMask _targetLayer;
        
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
            
            if (target && target.HasLayer(_targetLayer) && target.TryGetComponent(out HealthController health))
                health.Damage((int)_settings.attackDamage);
        }


        protected override void Exit()
        {
            animator.speed = 1;
        }
    }
}