using System.Linq;
using NPC.Brains;
using NPC.UnitData;
using NPC.Utility;
using Platoons;
using UnityEngine;

namespace NPC.Behaviours.Unit
{
    public class FindAllies : UnitBehaviour
    {
        [SerializeField] private float _forceMulti = 15f;

        
        public override void PhysicsUpdate()
        {
            if(!platoon) return;
            //  Finding all allies in sight

            var team = unitBrain.team;

            var position = transform.position;
            var ally = eyes.hits
                .Where(t => t.transform.TryGetComponent(out UnitBrain brain) && brain.platoon && brain.team == team)
                .OrderBy(t => (t.point - position).sqrMagnitude)
                .FirstOrDefault();
            
            ally.transform.GetComponent<UnitBrain>().platoon.AddUnit(unitBrain);
        }
    }
}