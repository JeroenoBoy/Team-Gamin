using UnityEngine;
using NPC.UnitData;

public class BehaviourMenu : MonoBehaviour
{
    //private UnitState settings => FindObjectOfType<UnitState>();
    public UnitState unitState;

    public void SetBehaviour(string unit)
    {
        if (UnitState.TryParse(unit, out UnitState state))
            unitState = state;
        else
            Debug.LogError("Invalid State!");
    }
}