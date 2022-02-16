using System.Linq;
using Controllers.Paths;
using NPC.UnitData;
using NPC.Utility;

namespace NPC.Behaviours.Unit
{
    public class WalkPath : UnitBehaviour
    {
        private int      _currentIndex;
        private int      _indexDirection;
        private PathNode _currentNode;
        private int      _pathLength;


        /**
         * Finds closest node when entering
         */
        protected override void Enter()
        {
            _pathLength     = stateController.path.totalNotes;
            _indexDirection = unitBrain.team == UnitTeam.Blue ? 1 : -1;
            
            //  Finding the closest node

            var position = transform.position;
            _currentNode = stateController.path.nodes
                .OrderBy(n => (n.position - position).sqrMagnitude)
                .First();

            _currentIndex = _currentNode.index;
        }


        /**
         * Walk towards the next node
         */
        public override void PhysicsUpdate()
        {
            //  Getting direction and distance
            
            var direction = _currentNode.position - transform.position;
            var distance = direction.sqrMagnitude;
            
            //  If the distance is smaller than a certain amount, change node

            if (distance < settings.minNodeDistance * settings.minNodeDistance)
                NextNode();
            
            //  Moving towards the node

            movement.AddForce(MoveWithArrive(_currentNode.position));
        }
        
        
        /**
         * Gets the next node
         */
        private void NextNode()
        {
            switch (_indexDirection)
            {
                case 1 when _currentIndex != _pathLength:
                    _currentIndex++;
                    break;
                
                
                case -1 when _currentIndex != 0:
                    _currentIndex--;
                    break;
            }
            
            _currentNode = stateController.path.GetNode(_currentIndex);
        }
    }
}