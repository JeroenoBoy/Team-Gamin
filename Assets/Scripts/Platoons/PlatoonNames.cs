using UnityEngine;
using TMPro;

public class PlatoonNames : MonoBehaviour
{
    private static readonly string[] names = {
        "The Jester Squadron",
        "Specialized Reconnaissance Unit",
        "Advanced Mobilization Team",
        "Billy Bobs Squad",
        "Bobs Billy Squad",
        "The Goblin boys",
        "The Handsome boyes",
        "Tactical Retaliation Team",
        "Classified Operation Squadron",
        "The Enigma Squad"
   };
    public TextMeshProUGUI test;

    private void Start()
    {
        SetPlatoonName();
    }

    public void SetPlatoonName()
    {
        int rand = Random.Range(0, names.Length);

        test.text = string.Format("{0}", names[rand]);
    }
}
