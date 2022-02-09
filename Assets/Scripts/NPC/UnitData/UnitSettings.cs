using System;
using Controllers;
using NPC.Brains;
using UnityEngine;

namespace NPC.UnitData
{
    public class UnitSettings : MonoBehaviour
    {
        public float movementSpeed;
        public UnitTeam team;
        [SerializeField] private UnitState _state;
        
        [Header("Attack")]
        public float attackSpeed;
        public int   attackDamage;
        public int   sightRange;
        
        [Header("Health")]
        public int baseHealth;
        public int defense;


        public UnitState state
        {
            get => _state;
            set
            {
                _state = value;
                SendMessage("OnStateChange");
            }
        }


        /**
         * Just to fix some bugs
         */
        public void Start()
        {
            //state = _state;
        }
    }
}