using System.Collections.Generic;
using UnityEngine;
using Game.Scripts.Utils;

public class TurnOffHealthBars : Singleton<TurnOffHealthBars>
{
    public bool toggled = false;
    public List<Canvas> canvasses = new List<Canvas>();    //list to store all canvasses where the healthbar and armorbar are in

    /// <summary>
    /// function to be called in an Unity Event with a button
    /// </summary>
    public void SetBool()
    {
        toggled = !toggled;

        //enables or disables the canvas list based on the toggled bool
        for (int i = 0; i < canvasses.Count; i++)
        {
            canvasses[i].enabled = toggled;
        }
    }
}
