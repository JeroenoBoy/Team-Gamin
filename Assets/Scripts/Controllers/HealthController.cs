using System;
using UnityEngine;

namespace Controllers
{
    public class HealthController : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private int _maxHealth;

        public int baseHealth;
        public bool isDead;
        
        
        #region Properties

        public int health
        {
            get => _health;
            set => ChangeHealth(value - _health);
        }

        public int maxHealth
        {
            get => _maxHealth;
            set => _maxHealth = value;
        }


        public int scaledDefence => Mathf.Clamp(_health - baseHealth, 0, maxDefence);
        public int scaledHealth  => Mathf.Clamp(_health, 0, baseHealth);
        public int maxDefence    => _maxHealth - baseHealth;

        #endregion


        private void OnEnable()
        {
            isDead = false;
            _health = _maxHealth;
        }


        /// <summary>
        /// Damage the component by a certain amount
        /// </summary>
        public int Damage(int amount)
        {
            if (amount < 0) throw new ArgumentException("Amount must not be lower than one");
            return -ChangeHealth(-amount);
        }
        

        /// <summary>
        /// Heal the component by a certain amount 
        /// </summary>
        public int Heal(int amount)
        {
            if (amount < 1) throw new ArgumentException("Amount must not be lower than one");
            return ChangeHealth(amount);
        }


        /// <summary>
        /// Chane the current health 
        /// </summary>
        private int ChangeHealth(int amount)
        {
            if (isDead) return 0;
            
            //  Setting the health based on the changed amount
            
            var newHealth = Mathf.Clamp(_health + amount, 0, _maxHealth);
            var changed   = newHealth - _health;
            _health = newHealth;
            
            //  Running events

            if (_health == 0)
            {
                isDead = true;
                SendMessage("OnDeath", SendMessageOptions.DontRequireReceiver);
            }
            
            else if(changed < 0) SendMessage("OnDamage", -changed, SendMessageOptions.DontRequireReceiver);
            else                 SendMessage("OnHeal",    changed, SendMessageOptions.DontRequireReceiver);

            //  Sending general HealthChange event
            
            SendMessage("HealthChange", changed, SendMessageOptions.DontRequireReceiver);

            //  Returning changed amount
            
            return changed;
        }
    }
}