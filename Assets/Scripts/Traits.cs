using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPC.UnitData;

[CreateAssetMenu]
public class Traits : ScriptableObject
{
    public TraitsClass[] TraitsClass;
}

[System.Serializable]
public class TraitsClass
{
    public string name;

    //values to assign in unity
    public float atkdmg;
    public float atkspd;
    public float movspd;
    public float defense;
    public float sightRange;
}
