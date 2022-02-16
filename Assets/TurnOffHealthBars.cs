using System.Collections.Generic;
using UnityEngine;
using Game.Scripts.Utils;

public class TurnOffHealthBars : Singleton<TurnOffHealthBars>
{
    public bool toggled = false;
    public List<Canvas> canvasses = new List<Canvas>();

    public void SetBool()
    {
        toggled = !toggled;

        for (int i = 0; i < canvasses.Count; i++)
        {
            canvasses[i].enabled = toggled;
        }
    }
}
