using UnityEngine;
using System;
using NPC.UnitData;
using UnityEngine.UI;

public class BehaviourMenu : MonoBehaviour
{
    [Header("States")]
    public UnitState unitState;
    public Button[] Button;

    [Header("Path")]
    public int pathIndex;
    public Button[] pathButtons;
    
    private int index;

    private void Start()
    {
        pathButtons[1].image.color = Button[0].image.color = Color.red;
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


    public void SetPath(int i)
    {
        foreach (var pathButton in pathButtons)
            pathButton.image.color = Color.black;

        pathButtons[i].image.color = Color.red;
        pathIndex = i;
    }
}