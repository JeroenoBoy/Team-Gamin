using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Controllers;
using UnityEngine;
using Util;

namespace NPC
{
    public abstract class AIBehavior : StateMachineBehaviour
    {
        protected bool started { get; private set; }

        
        #region Usefull properties for the behaviour

        protected StateController   stateController { get; private set; }
        protected Animator          animator        { get; private set; }
        protected AnimatorStateInfo stateInfo       { get; private set; }
        
        protected Transform target            => stateController.Target;
        protected Transform transform         => stateController.transform;
        protected NPCSettings settings        => stateController.Settings;
        protected MovementController movement => stateController.MovementController;

        #endregion
        
        
        //
        // These functions manage the state of this behaviour 
        //
        
        
        public override void OnStateEnter(Animator anim, AnimatorStateInfo stateInfo, int layerIndex)
        {
            this.stateInfo = stateInfo;
            if (!started) Init(anim);

            stateController.AddBehaviour(this);
            Enter();
        }
        
        


        public override void OnStateExit(Animator anim, AnimatorStateInfo stateInfo, int stateMachinePathHash)
        {
            this.stateInfo = stateInfo;
            if(!stateController) return;
            Exit();
            stateController.RemoveBehaviour(this);
            StopAllCoroutines();
        }


        /// <summary>
        /// Assigns the correct controller to this behaviour
        /// </summary>
        protected void Init(Animator anim)
        {
            started = true;
            stateController = anim.GetComponent<StateController>();
            animator = anim;
            
            if(!stateController) Debug.LogError("Animator does not have a StateController!");
            
            Start();
        }


        /// <summary>
        /// Get a component on the controller
        /// </summary>
        public T GetComponent<T>() => stateController.GetComponent<T>();


        #region Movement Utils

        /// <summary>
        /// Calculates the seek direction based on minSeekDistance
        /// </summary>
        protected Vector3 GetDirection(Vector3 direction, float speed = -1)
        {
            speed = speed < 0 ? movement.maxSpeed : speed;
            direction = direction.With(y: 0);
            
            //  Checking if the player is close enough to the target
         
            var distance = direction.sqrMagnitude;   
            return distance < settings.minSeekDistance * settings.minSeekDistance
                ? Vector3.zero
                : direction.ClampMagnitude(speed);
        }


        /// <summary>
        /// Move to a position with the arrive feature
        /// </summary>
        protected Vector3 MoveWithArrive(Vector3 targetPosition, float targetSpeed = -1)
        {
            targetSpeed = targetSpeed < 0 ? movement.maxSpeed : targetSpeed;

            var position = transform.position;

            //  Checking if the player is in stopping distance

            var direction = (targetPosition - position).With(y: 0);
            var distance = direction.magnitude;
            
            if(distance < settings.stopDistance)
                return Vector3.zero;
            
            //  Calculating the force based on slowing distance

            var slowDistance   = settings.slowDistance + settings.stopDistance;
            var currentSpeed = movement.velocity;

            var percentageDistance = distance / slowDistance;
            var desiredSpeed       = percentageDistance * targetSpeed;
            var desiredVel       = direction.normalized * desiredSpeed;
            
            return (desiredVel - currentSpeed).ClampMagnitude(targetSpeed);
        }
        
        #endregion


        public void Reset()
        {
            started = false;
        }


        #region Coroutines


        private readonly List<Coroutine> _coroutines = new List<Coroutine>();

        
        /**
         * Start a coroutine
         */
        protected Coroutine StartCoroutine(IEnumerator routine)
        {
            Coroutine coroutine = null;
            IEnumerator WrapCoroutine()
            {
                //  Executing coroutine
                var startTime = Time.time;
                yield return routine;
                
                //  Avoid potential error
                if (Math.Abs(startTime - Time.time) < 0.0001f) yield return null;
                
                //  Removing the coroutine
                _coroutines.Remove(coroutine);
            }
            
            //  Starting the coroutine
            
            coroutine = stateController.StartCoroutine(WrapCoroutine());
            _coroutines.Add(coroutine);
            
            return coroutine;
        }


        /**
         * Stop a single coroutine
         */
        protected void StopCoroutine(Coroutine coroutine)
        {
            stateController.StopCoroutine(coroutine);
            _coroutines.Remove(coroutine);
        }


        /**
         * Stop all coroutines
         */
        protected void StopAllCoroutines()
        {
            foreach (var coroutine in _coroutines)
                stateController.StopCoroutine(coroutine);
            _coroutines.Clear();
        }


        #endregion


        //
        //  Events
        //
        
        
        protected virtual void Start() {}
        protected virtual void Enter() {}
        public virtual void PhysicsUpdate() {}
        public virtual void OnDrawGizmos() {}
        protected virtual void Exit() {}
    }
}








