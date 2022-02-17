using UnityEngine;

[CreateAssetMenu]
public class Traits : ScriptableObject
{
    public TraitsClass[] TraitsClass;
}

[System.Serializable]
public class TraitsClass //class to store trait values
{
    
    public string name;

    //values to assign in unity
    public float atkdmg;
    public float atkspd;
    public float movspd;
    public float defense;
    public float sightRange;
}
