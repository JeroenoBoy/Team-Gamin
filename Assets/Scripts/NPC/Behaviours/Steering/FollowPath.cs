﻿// using System;
// using Controllers.Paths;
// using UnityEditor;
// using UnityEngine;
// using Random = UnityEngine.Random;
//
// namespace NPC.Behaviours.Steering
// {
//     public class FollowPath : AIBehavior
//     {
//         private int      _currentIndex;
//         private int      _indexDirection;
//         private PathNode _currentNode;
//         private int      _pathLength;
//
//
//         /// <summary>
//         /// Initializes script
//         /// </summary>
//         protected override void Enter()
//         {;
//             _pathLength     = stateController.path.totalNotes;
//             _indexDirection = 1;
//             
//             //  Setting the index based on the path type
//             _currentIndex = stateController.path.type == PathController.Type.Random
//                 ? Random.Range(0, _pathLength)
//                 : 0;
//             
//             _currentNode = stateController.path.GetNode(_currentIndex);
//         }
//
//
//         /// <summary>
//         /// Updates the movement
//         /// </summary>
//         public override void PhysicsUpdate()
//         {
//             //  Getting direction and distance
//             
//             var direction = _currentNode.position - transform.position;
//             var distance = direction.sqrMagnitude;
//             
//             //  If the distance is smaller than a certain amount, change node
//
//             if (distance < settings.minNodeDistance * settings.minNodeDistance)
//                 NextNode();
//             
//             //  Moving towards the node
//
//             movement.AddForce(MoveWithArrive(_currentNode.position));
//         }
//
//
//         /// <summary>
//         /// Update the current nodes
//         /// </summary>
//         private void NextNode()
//         {
//             switch (stateController.path.type)
//             {
//                 //  The loop Case
//                 
//                 case PathController.Type.Loop:
//                     
//                     if (_currentIndex >= _pathLength)
//                         _currentIndex = 0;
//                     else
//                         _currentIndex++;
//
//                     break;
//
//                 //  The normal case (Named normal because idk what to call it otherwise)
//                 
//                 case PathController.Type.ReverseLoop:
//                     
//                     if (_currentIndex <= 0)
//                         _currentIndex = _pathLength-1;
//                     else
//                         _currentIndex--;
//                     
//                     break;
//
//                 //  The normal case (Named normal because idk what to call it otherwise)
//                 
//                 case PathController.Type.PingPong:
//                     _currentIndex += _indexDirection;
//
//                     if (_currentIndex >= _pathLength || _currentIndex < 0)
//                     {
//                         _indexDirection *= -1;
//                         _currentIndex += _indexDirection * 2;
//                     }
//                     
//                     break;
//
//                 //  The normal case (Named normal because idk what to call it otherwise)
//                 
//                 case PathController.Type.Random:
//                     _currentIndex = Random.Range(0, _pathLength);
//                     break;
//                 
//                 default:
//                     throw new ArgumentOutOfRangeException();
//             }
//             
//             _currentNode = stateController.path.GetNode(_currentIndex);
//         }
//
//
//         /// <summary>
//         /// Draws the radius distance
//         /// </summary>
//         public override void OnDrawGizmos()
//         {
//             Handles.color = Color.yellow;
//             Handles.DrawWireDisc(transform.position, Vector3.up, settings.minNodeDistance);
//         }
//     }
// }