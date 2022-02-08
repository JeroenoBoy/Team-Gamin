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
            
            var allies = _eyes.hits
                .Where(t => t.transform.TryGetComponent(out UnitBrain brain) && brain.team == _team);

            //  Adding force towards allies in sight
            
            foreach (var hit in allies)
                movement.AddForce((hit.point - transform.position).normalized * _forceMulti);
            
            //  Checking for platoons
            
            PlatoonManager.instance.RequestPlatoon((UnitBrain)stateController);
        }
    }
}