using UnityEngine;

namespace Controllers
{
    public class HealthierController : HealthController
    {
        [Header("Bars")]
        [SerializeField] private Healthbar _healthBar;
        [SerializeField] private ArmorBar _armorBar;


        /**
         * Sets the defence & armor after everything has been binded
         */
        private void OnBind()
        {
            _healthBar.SetMaxHealth(baseHealth);

            if (_armorBar != null)
                _armorBar.SetMaxArmor(maxDefence);

            HealthChange();
        }

        
        /**
         * Updates the bars
         */
        private void HealthChange()
        {
            _healthBar.SetHealth(scaledHealth);

            if (_armorBar != null)
                _armorBar.SetArmor(scaledDefence);
        }

    }
}