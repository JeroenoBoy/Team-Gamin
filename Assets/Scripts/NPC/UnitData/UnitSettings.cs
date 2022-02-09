using System;
using Controllers;
using Controllers.Paths;
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


        public Transform targetCastle
        {
            get => _brain.castleTarget;
            set => _brain.castleTarget = value;
        }


        public PathController path
        {
            get => _brain.path;
            set => _brain.path = value;
        }


        public UnitState state
        {
            get => _state;
            set
            {
                _state = value;
                SendMessage("OnStateChange");
            }
        }


        private UnitBrain _brain;


        private void Awake()
        {
            _brain = GetComponent<UnitBrain>();
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