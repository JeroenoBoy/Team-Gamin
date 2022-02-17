using System;
using Controllers;
using Controllers.Paths;
using NPC.Brains;
using UnityEngine;

namespace NPC.UnitData
{
    public class UnitSettings : MonoBehaviour
    {
        public UnitTeam team;
        [SerializeField] private UnitState _state;

        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _attackSpeed;
        [SerializeField] private int   _attackDamage;
        [SerializeField] private int   _sightRange;
        [SerializeField] private int   _defense;

        [Header("Base stats")]
        [SerializeField] private float _baseSpeed;
        [SerializeField] private float _baseAttackSpeed;
        [SerializeField] private float _baseDamage;
        [SerializeField] private float _baseSightRange;
        [SerializeField] private float _baseDefence;

        [Header("Multiplies")]
        [SerializeField] private float _speedMulti;
        [SerializeField] private float _dmgMulti;
        [SerializeField] private float _attackSpeedMulti;
        [SerializeField] private float _sightMulti;
        [SerializeField] private float _defenceMutli;
        
        
        #region Properties
        
        
        public float movementSpeed
        {
            get => _movementSpeed;
            set => _movementSpeed = _baseSpeed + value * _speedMulti;
        }


        public float attackSpeed
        {
            get => _attackSpeed;
            set => _attackSpeed = _baseAttackSpeed - value * _attackSpeedMulti;
        }


        public float attackDamage
        {
            get => _attackDamage;
            set => _attackDamage = (int)(_baseDamage + value * _dmgMulti);
        }


        public float sightRange
        {
            get => _sightRange;
            set => _sightRange = (int)(_baseSightRange + value * _sightMulti);
        }


        public float defense
        {
            get => _defense;
            set => _defense = (int)(_baseDefence + value * _defenceMutli);
        }


        public int baseDefence => (int)_baseDefence;

        #endregion

        
        #region Behaviour Stats

        
        /**
         * Set the target castle
         */
        public Transform targetCastle
        {
            get => _brain.castleTarget;
            set => _brain.castleTarget = value;
        }


        /**
         * The path I should follow
         */
        public PathController path
        {
            get => _brain.path;
            set => _brain.path = value;
        }


        /**
         * The behaviour I should use
         */
        public UnitState state
        {
            get => _state;
            set
            {
                _state = value;
                SendMessage("OnStateChange");
            }
        }

        #endregion
        

        private UnitBrain _brain;


        private void Awake()
        {
            _brain = GetComponent<UnitBrain>();
        }


        public void Bind()
        {
            _brain.Bind();
        }
    }
}