using Controllers;
using NPC.Brains;
using NPC.UnitData;
using Platoons;

namespace NPC.Utility
{
    public class UnitBehaviour : AIBehavior
    {
        protected UnitBrain        unitBrain => (UnitBrain)stateController;
        protected UnitSettings     unitSettings => unitBrain.unitSettings;
        protected HealthController healthController => unitBrain.healthComponent;
        protected Eyes eyes => unitBrain.eyes;

        
        protected Platoon platoon
        {
            get => unitBrain.platoon;
            set => unitBrain.platoon = value;
        }
    }
}