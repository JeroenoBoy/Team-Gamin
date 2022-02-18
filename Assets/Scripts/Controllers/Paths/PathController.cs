using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Util;

namespace Controllers.Paths
{
    public class PathController : MonoBehaviour, IEnumerable
    {
        public enum PathType { PingPong, Loop, ReverseLoop, Random }
        [SerializeField] private PathType   _type;
        [SerializeField] private bool       _autoPath;
        [SerializeField] private bool       _revealNodes;
        [SerializeField] private PathNode[] _nodes;

        public int TotalNotes => _nodes.Length;
        public PathType Type => _type;
        public PathNode[] Nodes => _nodes;


        /**
         * Collect all nodes when i awake
         */
        private void Awake()
        {
            CollectNodes();
        }


        /**
         * Gets all the nodes in the children
         */
        private void CollectNodes()
        {
            if (!_autoPath) return;
            
            _nodes = GetComponentsInChildren<PathNode>();
            for (var i = 0; i < _nodes.Length; i++)
                _nodes[i].Index = i;
        }


        
        #if UNITY_EDITOR
        //  This bit of code is something i want to experiment with
        //  that should make debugging a lot easier.
        //
        //  Looking back at it its horribly inefficient, but it does
        //  the job & its is editor only.

        
        /**
         * Update the node collection
         */
        public void NodeAdded() => CollectNodes();

        
        /**
         * Checks if a node contains this node
         */
        public bool Contains(PathNode node) => _nodes.Contains(node);


        /**
         * Reveals the nodes when the input changes
         */
        private void OnValidate()
        {
            if (_revealNodes) CollectNodes();
        }
        
        #endif
        
        
        /**
         * Get the node at an certain index
         */
        public PathNode GetNode(int index) => _nodes[index % _nodes.Length];


        /**
         * Draws all the nodes in the object
         */
        private void OnDrawGizmos()
        {
            if(!_revealNodes) return;
            if(_nodes.Length == 0) return;

            //  Drawing the first and "previous" node
            
            var previous = _nodes[0];
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(previous.Position, 0.5f);
            
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

                if (_type != PathType.Random)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(node.Position, previous.Position);   
                }
                
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(node.Position, 0.5f);

                previous = node;
            }
            
            //  Drawing a line between the last and first if looping is true
            
            if(_type != PathType.Loop && _type != PathType.ReverseLoop) return;
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_nodes[0].Position, previous.Position);
        }
        
        
        /**
         * Get all nodes
         */
        public IEnumerator GetEnumerator() => _nodes.GetEnumerator();
    }
}