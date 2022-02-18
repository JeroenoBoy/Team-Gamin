using System;
using NPC.UnitData;
using UnityEngine;

namespace Controllers
{
    public class HealthController : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private int _maxHealth;
        [SerializeField] private float _blockingRange;

        public int baseHealth;
        public bool isDead;
        public bool IsBlocking;
        

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


        /**
         * Reset death & health
         */
        private void OnEnable()
        {
            isDead = false;
            _health = _maxHealth;
        }


        /**
         * Damage the component by a certain amount
         */
        public int Damage(int amount, Vector3? point = null)
        {
            if (amount < 0) throw new ArgumentException("Amount must not be lower than one");

            //  Checks for blocking

            switch (IsBlocking)
            {
                //  Start blocking
                case true when point == null:
                case true when Vector3.Angle(transform.forward, (Vector3) point - transform.position) <= _blockingRange:
                    return 0;
                
                //  Do default damage
                default:
                    return ChangeHealth(-amount);
            }
        }
        

        /**
         * Heal the component by a certain amount 
         */
        public int Heal(int amount)
        {
            if (amount < 1) throw new ArgumentException("Amount must not be lower than one");
            return ChangeHealth(amount);
        }


        /**
         * Chane the current health 
         */
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