using System.Collections.Generic;
using System.Linq;
using Controllers;
using NPC.Brains;
using NPC.UnitData;
using UnityEngine;

namespace Platoons
{
    public class Platoon
    {
        public readonly UnitTeam Team;
        public readonly List<PlatoonController> Units = new List<PlatoonController>();

        public int Count => Units.Count;
        public PlatoonController Master => Units[0];
        

        /**
         * Creates a new platoon
         */
        public Platoon(UnitTeam team, PlatoonController controller)
        {
            Team = team;
            AddUnit(controller);

            PlatoonManager.instance.AddPlatoon(this);
        }


        /**
         * Migrates this platoon to another platoon
         */
        public void MigratePlatoon(Platoon targetPlatoon)
        {
            while (Units.Count > 0) targetPlatoon.AddUnit(Units.First());
            PlatoonManager.instance.RemovePlatoon(this);
        }


        /**
         * Migrates this platoon to another platoon
         */
        public void AddUnit(PlatoonController unit)
        {
            if (unit.platoon) unit.platoon.RemoveUnit(unit);
            unit.platoon = this;
            Units.Add(unit);
            
            foreach (var unitBrain in Units)
                unitBrain.SendMessage("OnPlatoonUpdate", SendMessageOptions.RequireReceiver);
        }


        /**
         * Migrates this platoon to another platoon
         */
        public void RemoveUnit(PlatoonController unit)
        {
            unit.platoon = null;
            Units.Remove(unit);
        }


        /**
         * Send a message to all units in this platoon
         */
        public void SendMessage(string name, object obj = null)
        {
            foreach (var platoonController in Units)
                platoonController.gameObject.SendMessage(name, obj);
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