using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Controllers;
using Controllers.Paths;
using UnityEditor.Animations;
using UnityEngine;

namespace NPC
{
    [RequireComponent(typeof(Animator))]
    public abstract class StateController : MonoBehaviour
    {
        [SerializeField] private Transform      _target;
        [SerializeField] private NPCSettings    _settings;
        public PathController path;
        
        [SerializeField] private AIBehavior[] _behaviours;
        
        
        #region Getters & Setters

        public MovementController movementController { get; private set; }
        public Animator animator { get; private set; }

        /// <summary>
        /// Get the target of this controller
        /// </summary>
        public virtual Transform target
        {
            get => _target;
            set => _target = value;
        }

        /// <summary>
        /// Get the settings of this controller
        /// </summary>
        public NPCSettings settings => _settings;

        #endregion


        protected virtual void Awake()
        {
            movementController = GetComponent<MovementController>();
            animator = GetComponent<Animator>();
        }


        /// <summary>
        /// Add a behaviour & start a behaviour
        /// </summary>
        public void AddBehaviour(AIBehavior behavior)
        {
            _behaviours = _behaviours.Append(behavior).ToArray();
        }
    
        
        /// <summary>
        /// Remove a behaviour
        /// </summary>
        public void RemoveBehaviour(AIBehavior behavior)
        {
            _behaviours = _behaviours.Where(b => b != behavior).ToArray();
        }


        /// <summary>
        /// Send PhysicsUpdates to all behaviors
        /// </summary>
        protected virtual void FixedUpdate()
        {
            foreach (var behaviour in _behaviours)
            {
                behaviour.PhysicsUpdate();
            }
        }


        protected void Reset()
        {
            _behaviours = Array.Empty<AIBehavior>();
            animator.Rebind();
            animator.Update(0f);
            movementController.canMove = true;
        }


        /// <summary>
        /// Execute the OnDrawGizmos in the behaviours
        /// </summary>
        protected virtual void OnDrawGizmos()
        {
            foreach (var behaviour in _behaviours)
            {
                behaviour.OnDrawGizmos();
            }
        }
    }
}
