using UnityEngine;
using Game.Scripts.Utils;
using TMPro;
using NPC.UnitData;

public class TraitSetter : Singleton<TraitSetter>
{
    public UnitSettings settings;

    public GameObject TraitsPanel;

    public TextMeshProUGUI team;
    public TextMeshProUGUI trait;
    public TextMeshProUGUI atkdmg;
    public TextMeshProUGUI atkspd;
    public TextMeshProUGUI movspd;
    public TextMeshProUGUI defence;
    public TextMeshProUGUI sightrange;

    public void SetTraitText()
    {
        atkdmg.text = string.Format("Attack Damage + {0}", settings.attackDamage);
        atkspd.text = string.Format("Attack Speed + {0}", settings.attackSpeed);
        movspd.text = string.Format("Move Speed + {0}", settings.movementSpeed);
        defence.text = string.Format("Defence + {0}", settings.defense);
        sightrange.text = string.Format("Sight Range + {0}", settings.sightRange);
    }
}
