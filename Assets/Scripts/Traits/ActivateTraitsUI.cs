using UnityEngine;

public class ActivateTraitsUI : MonoBehaviour
{
    
    public void OnMouseDown()
    {
        TraitSetter.instance.SetTraitText();
        TraitSetter.instance.TraitsPanel.SetActive(true);
    }
}
