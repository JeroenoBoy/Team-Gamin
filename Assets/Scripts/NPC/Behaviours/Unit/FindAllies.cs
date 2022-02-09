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
        
        private UnitTeam _team => ((UnitBrain)stateController).team;
        
        protected override void Start()
        {
            _eyes = GetComponent<Eyes>();
        }


        public override void PhysicsUpdate()
        {
            //  Finding all allies in sight

            var position = transform.position;
            var ally = _eyes.hits
                .Where(t => t.transform.TryGetComponent(out UnitBrain brain) && brain.team == _team)
                .OrderBy(t => (t.point - position).sqrMagnitude)
                .FirstOrDefault();
            
            if(!ally.transform) return;
            ally.transform.GetComponent<UnitBrain>().platoon.AddUnit((UnitBrain)stateController);
        }
    }
}