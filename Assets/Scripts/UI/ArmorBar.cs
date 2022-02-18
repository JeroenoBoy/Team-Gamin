using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ArmorBar : MonoBehaviour
    {
        private Slider slider;
        [SerializeField] private Gradient gradient;
        [SerializeField] private Image fill;

        private void Awake()
        {
            slider = GetComponent<Slider>();
        }

        /// <summary>
        /// function to set the armorbar max value
        /// </summary>
        /// <param name="armor"></param>
        public void SetMaxArmor(int armor)
        {
            slider.maxValue = armor;
            slider.value = armor;

            fill.color = gradient.Evaluate(1f);
        }

        /// <summary>
        /// function to update the armorbar slider
        /// </summary>
        /// <param name="armor"></param>
        public void SetArmor(int armor)
        {
            slider.value = armor;

            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
    }
}
