using System.Collections.Generic;
using System.Linq;
using NPC.Brains;
using NPC.UnitData;
using UnityEngine;

namespace Platoons
{
    public class Platoon
    {
        public readonly UnitTeam team;
        public readonly List<UnitBrain> units = new List<UnitBrain>();

        public int Count => units.Count;


        /**
         * Creates a new platoon
         */
        public Platoon(UnitTeam team, UnitBrain brain)
        {
            this.team = team;
            AddUnit(brain);

            PlatoonManager.instance.AddPlatoon(this);
        }


        /**
         * Migrates this platoon to another platoon
         */
        public void MigratePlatoon(Platoon targetPlatoon)
        {
            while (units.Count > 0) targetPlatoon.AddUnit(units.First());
            PlatoonManager.instance.RemovePlatoon(this);
        }


        /**
         * Migrates this platoon to another platoon
         */
        public void AddUnit(UnitBrain unit)
        {
            if (unit.platoon) unit.platoon.RemoveUnit(unit);
            unit.platoon = this;
            units.Add(unit);
            
            foreach (var unitBrain in units)
                unitBrain.SendMessage("OnPlatoonUpdate", SendMessageOptions.RequireReceiver);
        }


        /**
         * Migrates this platoon to another platoon
         */
        public void RemoveUnit(UnitBrain unit)
        {
            unit.platoon = null;
            units.Remove(unit);
        }

        
        /**
         * Simple null checks
         */
        public static bool operator !(Platoon self) => self == null;

        /**
         * Check if the platoon exists
         */
        public static implicit operator bool(Platoon self) => self != null;
    }
}