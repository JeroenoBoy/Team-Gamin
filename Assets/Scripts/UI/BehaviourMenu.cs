using UnityEngine;
using System;
using NPC.UnitData;
using UnityEngine.UI;

public class BehaviourMenu : MonoBehaviour
{
    public UnitState unitState;
    public Button[] Button;
    private int index;

    private void Start()
    {
        Button[0].image.color = Color.red;
    }

    public void SetBehaviour(string unit)
    {
        for (int i = 0; i < Button.Length; i++)
        {
            Button[i].image.color = Color.black;
        }

        if (UnitState.TryParse(unit, out UnitState state))
            unitState = state;
        else
            Debug.LogError("Invalid State!");

        index = Array.IndexOf(UnitStateExtensions.states, unitState);
        Button[index].image.color = Color.red;
    }
}