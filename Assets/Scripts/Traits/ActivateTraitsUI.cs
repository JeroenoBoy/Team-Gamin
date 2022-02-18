using NPC.UnitData;
using UnityEngine;

namespace Traits
{
    public class ActivateTraitsUI : MonoBehaviour
    {
        private UnitSettings _settings;

        
        private void Start()
        {
            _settings = GetComponent<UnitSettings>();
        }

        
        /**
         * Detects when clicked upon
         */
        public void OnMouseDown()
        {
            //  When clicked on a unit call the set text function and turn on the panel in the ui
            TraitSetter.Instance.SetTraitText(_settings);
            TraitSetter.Instance.TraitsPanel.SetActive(true);
        }
    }
}
