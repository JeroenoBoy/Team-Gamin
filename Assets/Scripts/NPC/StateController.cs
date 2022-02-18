using System;
using System.Linq;
using Controllers;
using Controllers.Paths;
using UnityEngine;
using UnityEngine.Serialization;

namespace NPC
{
    [RequireComponent(typeof(Animator))]
    public abstract class StateController : MonoBehaviour
    {
        [SerializeField] private Transform      _target;
        [SerializeField] private NPCSettings    _settings;
        [SerializeField] public  PathController Path;
        [Space]
        [SerializeField] private AIBehavior[] _behaviours;
        
        public MovementController MovementController { get; private set; }
        public Animator Animator { get; private set; }
        
        
        #region Properties
        
        /**
         * Get the target of this controller
         */
        public virtual Transform Target
        {
            get => _target;
            set => _target = value;
        }

        /**
         * Get the settings of this controller
         */
        public NPCSettings Settings => _settings;

        #endregion


        protected virtual void Awake()
        {
            MovementController = GetComponent<MovementController>();
            Animator = GetComponent<Animator>();
        }


        /**
         * Add a behaviour & start a behaviour
         */
        public void AddBehaviour(AIBehavior behavior)
        {
            _behaviours = _behaviours.Append(behavior).ToArray();
        }
    
        
        /**
         * Remove a behaviour
         */
        public void RemoveBehaviour(AIBehavior behavior)
        {
            _behaviours = _behaviours.Where(b => b != behavior).ToArray();
        }


        /**
         * Send PhysicsUpdates to all behaviors
         */
        protected virtual void FixedUpdate()
        {
            foreach (var behaviour in _behaviours)
                behaviour.PhysicsUpdate();
        }


        /**
         * Reset the behaviour
         */
        protected void ResetUnit()
        {
            _behaviours = Array.Empty<AIBehavior>();
            Animator.Rebind();
            MovementController.CanMove = true;
        }


        /**
         * Execute the OnDrawGizmos in the behaviours
         */
        protected virtual void OnDrawGizmos()
        {
            foreach (var behaviour in _behaviours)
                behaviour.OnDrawGizmos();
        }
    }
}
