using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ArmorBar : MonoBehaviour
    {
        [SerializeField] private Gradient gradient;
        [SerializeField] private Image fill;
        
        private Slider _slider;

        
        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        
        /// <summary>
        /// function to set the armorbar max value
        /// </summary>
        public void SetMaxArmor(int armor)
        {
            _slider.maxValue = armor;
            _slider.value = armor;

            fill.color = gradient.Evaluate(1f);
        }

        
        /// <summary>
        /// function to update the armorbar slider
        /// </summary>
        public void SetArmor(int armor)
        {
            _slider.value = armor;

            fill.color = gradient.Evaluate(_slider.normalizedValue);
        }
    }
}
