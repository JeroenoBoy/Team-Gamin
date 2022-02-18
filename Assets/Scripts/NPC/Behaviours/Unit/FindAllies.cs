using System.Linq;
using Controllers;
using NPC.Brains;
using NPC.UnitData;
using NPC.Utility;
using Platoons;
using UnityEngine;

namespace NPC.Behaviours.Unit
{
    public class FindAllies : UnitBehaviour
    {
        private PlatoonController _platoonController;
        
        protected override void Start()
        {
            _platoonController = GetComponent<PlatoonController>();
        }


        public override void PhysicsUpdate()
        {
            if (!platoon) return;
            //  Finding all allies in sight

            var team = unitBrain.Team;
            var position = transform.position;
            var ally = eyes.Hits
                .Where(t => t.transform.TryGetComponent(out UnitBrain brain) && brain.Platoon && brain.Team == team)
                .OrderBy(t => (t.point - position).sqrMagnitude)
                .FirstOrDefault();

            if(ally.transform == null) return;
            ally.transform.GetComponent<UnitBrain>().Platoon.AddUnit(_platoonController);
        }
    }
}