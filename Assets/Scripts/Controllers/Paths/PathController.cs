using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;

namespace Controllers.Paths
{
    public class PathController : MonoBehaviour
    {
        public enum Type { PingPong, Loop, ReverseLoop, Random }
        [SerializeField] private Type       _type;
        [SerializeField] private bool       _autoPath;
        [SerializeField] private bool       _revealNodes;
        [SerializeField] private PathNode[] _nodes;

        public int totalNotes => _nodes.Length;
        public Type type => _type;


        /// <summary>
        /// Collect all nodes when i awake
        /// </summary>
        private void Awake()
        {
            CollectNodes();
        }


        /// <summary>
        /// Gets all the nodes in the children
        /// </summary>
        private void CollectNodes()
        {
            if (_autoPath) _nodes = GetComponentsInChildren<PathNode>();
        }


        
        #if UNITY_EDITOR
        //  This bit of code is something i want to experiment with
        //  that should make debugging a lot easier.
        //
        //  Looking back at it its horribly inefficient, but it does
        //  the job & its is editor only.

        
        /// <summary>
        /// Update the node collection
        /// </summary>
        public void NodeAdded()
            => CollectNodes();

        
        /// <summary>
        /// Checks if a node contains this node
        /// </summary>
        public bool Contains(PathNode node)
            => _nodes.Contains(node);


        /// <summary>
        /// Reveals the nodes when the input changes
        /// </summary>
        private void OnValidate()
        {
            if (_revealNodes) CollectNodes();
        }
        
        #endif
        
        
        /// <summary>
        /// Get the node at an certain index
        /// </summary>
        public PathNode GetNode(int index)
            => _nodes[index % _nodes.Length];


        /// <summary>
        /// Draws all the nodes in the object
        /// </summary>
        private void OnDrawGizmos()
        {
            if(!_revealNodes) return;
            if(_nodes.Length == 0) return;

            //  Drawing the first and "previous" node
            
            var previous = _nodes[0];
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(previous.position, 0.5f);
            
            //  Drawing spheres with lines
            
            for(var i = 1; i < _nodes.Length; i++)
            {
                var node = _nodes[i];

                //  Checking if the object has been destroyed
                
                if (!node)
                {
                    CollectNodes();
                    return;
                }
                
                //  Draw the gizmos

                if (_type != Type.Random)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(node.position, previous.position);   
                }
                
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(node.position, 0.5f);

                previous = node;
            }
            
            //  Drawing a line between the last and first if looping is true
            
            if(_type != Type.Loop && _type != Type.ReverseLoop) return;
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_nodes[0].position, previous.position);
        }
    }
}