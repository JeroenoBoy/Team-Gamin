using UnityEngine;
using UnityEngine.UI;

public class ArmorBar : MonoBehaviour
{
    private Slider slider => GetComponent<Slider>();
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;

  
    public void SetMaxArmor(int armor)
    {
        slider.maxValue = armor;
        slider.value = armor;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetArmor(int armor)
    {
        slider.value = armor;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
