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
        //gives the starting behaviour the color red to indicate it being selected
        pathButtons[1].image.color = Button[0].image.color = Color.red;
    }

    /// <summary>
    /// function to set behaviour to what behaviour you clicked on in the ui, call this function with a button
    /// </summary>
    /// <param name="unit"></param>
    public void SetBehaviour(string unit)
    {
        //sets all the buttons color to black
        for (int i = 0; i < Button.Length; i++)
        {
            Button[i].image.color = Color.black;
        }

        //changes string to enum, returns the state
        if (UnitState.TryParse(unit, out UnitState state))
            unitState = state;
        else
            Debug.LogError("Invalid State!");

        index = Array.IndexOf(UnitStateExtensions.states, unitState);

        //sets selected button to red
        Button[index].image.color = Color.red;
    }

    /// <summary>
    /// function to set the path, call this with a button in the UI
    /// </summary>
    /// <param name="i"></param>
    public void SetPath(int i)
    {
        foreach (var pathButton in pathButtons)
            pathButton.image.color = Color.black;

        pathButtons[i].image.color = Color.red;
        pathIndex = i;
    }
}