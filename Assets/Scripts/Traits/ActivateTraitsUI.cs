using NPC.UnitData;
using UnityEngine;

namespace Traits
{
    public class ActivateTraitsUI : MonoBehaviour
    {
        private UnitSettings settings;

        private void Start()
        {
            settings = GetComponent<UnitSettings>();
        }

        public void OnMouseDown()
        {
            //when clicked on a unit call the set text function and turn on the panel in the ui
            TraitSetter.instance.SetTraitText(settings);
            TraitSetter.instance.TraitsPanel.SetActive(true);
        }
    }
}
