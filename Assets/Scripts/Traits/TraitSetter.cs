using NPC.UnitData;
using TMPro;
using UnityEngine;
using Util;

namespace Traits
{
    public class TraitSetter : Singleton<TraitSetter>
    {
        //references
        public GameObject TraitsPanel;

        //text objects
        public TextMeshProUGUI Trait;
        public TextMeshProUGUI AtkDmg;
        public TextMeshProUGUI AtkSpd;
        public TextMeshProUGUI MovSpd;
        public TextMeshProUGUI Defence;
        public TextMeshProUGUI SightRange;

        /// <summary>
        /// function to set the trait text, call this in a function when clicking on a unit
        /// </summary>
        /// <param name="settings"></param>
        public void SetTraitText(UnitSettings settings)
        {
            //sets the current trait text
            Trait.text = "Trait : " + settings.Traits;

            //sets the stats text
            AtkDmg.text = string.Format("ATK DMG : {0}", settings.AttackDamage.ToString("F"));
            AtkSpd.text = string.Format("ATK SPD : {0}", settings.AttackSpeed.ToString("F"));
            MovSpd.text = string.Format("Speed : {0}", settings.MovementSpeed.ToString("F"));
            Defence.text = string.Format("Defence : {0}", settings.Defense.ToString("F"));
            SightRange.text = string.Format("Sight : {0}", settings.SightRange.ToString("F"));
        }
    }
}
