using System.Collections.Generic;
using NPC.Brains;
using NPC.UnitData;

namespace Platoons
{
    public class Platoon
    {
        public readonly UnitTeam team;
        public readonly List<UnitBrain> units;

        public int Count => units.Count;


        /**
         * Creates a new platoon
         */
        public Platoon(UnitTeam team, UnitBrain brain)
        {
            this.team = team;
            units = new List<UnitBrain>() { brain };

            PlatoonManager.instance.AddPlatoon(this);
        }


        /**
         * Migrates this platoon to another platoon
         */
        public void MigratePlatoon(Platoon targetPlatoon)
        {
            foreach (var unitBrain in units)
                unitBrain.platoon = targetPlatoon;
            
            PlatoonManager.instance.RemovePlatoon(this);
        }


        /**
         * Migrates this platoon to another platoon
         */
        public void AddUnit(UnitBrain unit)
        {
            if (unit.platoon != null) unit.platoon.RemoveUnit(unit);
            unit.platoon = this;
            units.Add(unit);
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
    }
}