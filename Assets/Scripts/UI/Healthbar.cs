using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Healthbar : MonoBehaviour
    {
        private Slider slider;
        [SerializeField] private Gradient gradient;
        [SerializeField] private Image fill;
        [SerializeField] private Canvas canvas;

        private void Awake()
        {
            slider = GetComponent<Slider>();
        }

        
        /// <summary>
        /// function to set the max value of the healthbar
        /// </summary>
        /// <param name="health"></param>
        public void SetMaxHealth(int health)
        {
            slider.maxValue = health;
            slider.value = health;

            fill.color = gradient.Evaluate(1f);
        }

        
        /// <summary>
        /// function to update the healthbar slider, call this when units take damage
        /// </summary>
        /// <param name="health"></param>
        public void SetHealth(int health)
        {
            slider.value = health;

            fill.color = gradient.Evaluate(slider.normalizedValue);
        }

        
        private void OnEnable()
        {
            //adds the healthbar canvas to a canvas list
            TurnOffHealthBars.Instance.canvasses.Add(canvas);
            canvas.enabled = TurnOffHealthBars.Instance.toggled;
        }

        
        private void OnDisable()
        {
            if (TurnOffHealthBars.Instance)
            {
                //removes the canvas from canvas list
                TurnOffHealthBars.Instance.canvasses.Remove(canvas);
            }
        }
    }
}
