using System.Linq;
using NPC.Brains;
using NPC.UnitData;
using NPC.Utility;
using Platoons;
using UnityEngine;

namespace NPC.Behaviours.Unit
{
    public class FindAllies : AIBehavior
    {
        [SerializeField] private float _forceMulti = 15f;
        
        private Eyes _eyes;

        private UnitBrain _brain => (UnitBrain)stateController;
        private UnitTeam   _team => _brain.team;
        private Platoon _platoon => _brain.platoon;
        
        protected override void Start()
        {
            _eyes = GetComponent<Eyes>();
        }


        public override void PhysicsUpdate()
        {
            if(!_platoon) return;
            //  Finding all allies in sight

            var position = transform.position;
            var ally = _eyes.hits
                .Where(t => t.transform.TryGetComponent(out UnitBrain brain) && brain.platoon && brain.team == _team)
                .OrderBy(t => (t.point - position).sqrMagnitude)
                .FirstOrDefault();
            
            ally.transform.GetComponent<UnitBrain>().platoon.AddUnit(_brain);
        }
    }
}