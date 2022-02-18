using Controllers;
using NPC.Brains;
using NPC.UnitData;
using Platoons;

namespace NPC.Utility
{
    public class UnitBehaviour : AIBehavior
    {
        protected UnitBrain        unitBrain => (UnitBrain)stateController;
        protected UnitSettings     unitSettings => unitBrain.UnitSettings;
        protected HealthController healthController => unitBrain.HealthComponent;
        protected Eyes eyes => unitBrain.Eyes;

        
        protected Platoon platoon
        {
            get => unitBrain.Platoon;
            set => unitBrain.Platoon = value;
        }
    }
}