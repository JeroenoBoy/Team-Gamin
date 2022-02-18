using System;
using Controllers.Paths;
using UnityEditor;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

namespace NPC.Behaviours.Steering
{
    public class FollowPath : AIBehavior
    {
        private int      _currentIndex;
        private int      _indexDirection;
        private PathNode _currentNode;
        private int      _pathLength;


        /// <summary>
        /// Initializes script
        /// </summary>
        protected override void Enter()
        {;
            _pathLength     = stateController.Path.TotalNotes;
            _indexDirection = 1;
            
            //  Finding the closest node

            var position = transform.position;
            _currentNode = stateController.Path.Nodes
                .OrderBy(n => (n.Position - position).sqrMagnitude)
                .First();

            _currentIndex = _currentNode.Index;
        }


        /// <summary>
        /// Updates the movement
        /// </summary>
        public override void PhysicsUpdate()
        {
            //  Getting direction and distance
            
            var direction = _currentNode.Position - transform.position;
            var distance = direction.sqrMagnitude;
            
            //  If the distance is smaller than a certain amount, change node

            if (distance < settings.minNodeDistance * settings.minNodeDistance)
                NextNode();
            
            //  Moving towards the node

            movement.AddForce(MoveWithArrive(_currentNode.Position));
        }


        /// <summary>
        /// Update the current nodes
        /// </summary>
        private void NextNode()
        {
            switch (stateController.Path.Type)
            {
                //  The loop Case
                
                case PathController.PathType.Loop:
                    
                    if (_currentIndex >= _pathLength)
                        _currentIndex = 0;
                    else
                        _currentIndex++;

                    break;

                //  The normal case (Named normal because idk what to call it otherwise)
                
                case PathController.PathType.ReverseLoop:
                    
                    if (_currentIndex <= 0)
                        _currentIndex = _pathLength-1;
                    else
                        _currentIndex--;
                    
                    break;

                //  The normal case (Named normal because idk what to call it otherwise)
                
                case PathController.PathType.PingPong:
                    _currentIndex += _indexDirection;

                    if (_currentIndex >= _pathLength || _currentIndex < 0)
                    {
                        _indexDirection *= -1;
                        _currentIndex += _indexDirection * 2;
                    }
                    
                    break;

                //  The normal case (Named normal because idk what to call it otherwise)
                
                case PathController.PathType.Random:
                    _currentIndex = Random.Range(0, _pathLength);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _currentNode = stateController.Path.GetNode(_currentIndex);
        }


#if UNITY_EDITOR
        /// <summary>
        /// Draws the radius distance
        /// </summary>
        public override void OnDrawGizmos()
        {
            Handles.color = Color.yellow;
            Handles.DrawWireDisc(transform.position, Vector3.up, settings.minNodeDistance);
        }
#endif
    }
}