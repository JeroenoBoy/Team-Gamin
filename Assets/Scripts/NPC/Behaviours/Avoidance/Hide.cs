using System.Linq;
using UnityEngine;

namespace NPC.Behaviours.Avoidance
{
    public class Hide : AIBehavior
    {
        private float     _nextLook;
        private Transform _targetObstacle;
        private Vector3   _targetPos;
        

        public override void PhysicsUpdate()
        {
            if(!target) return;
            if (_nextLook < Time.time) FindObstacles();
            if (!_targetObstacle) return;

            //  Getting target position

            var targetPos   = target.position;
            var obstaclePos = _targetObstacle.position;
            var direction   = (obstaclePos - targetPos).normalized;
            var relativePos = direction * _targetObstacle.lossyScale.magnitude * .5f + direction * settings.HideTargetDistance;
            
            //  Move towards the target with arrive

            _targetPos = obstaclePos + relativePos;
            movement.AddForce(MoveWithArrive(_targetPos));
        }


        /// <summary>
        /// Find an obstacle near the player
        /// </summary>
        private void FindObstacles()
        {
            //  Find all nearby obstacles
            
            var distance = settings.HideMaxDistance;
            var mask = settings.HideMask;
                
            var colliders = Physics.OverlapSphere(transform.position, distance, mask);
            
            if (colliders.Length > 0)
            {
                //  Finding closest target

                var position = transform.position;
                var sortedColliders = colliders
                    .OrderBy(b => (b.transform.position - position).sqrMagnitude)
                    .ToArray();

                //  Assign nextLook & moveTarget
            
                _targetObstacle = sortedColliders[0].transform;
            }
            else
            {
                _targetObstacle = null;
            }
            
            _nextLook = Time.time + settings.HideLookInterval;
        }


        /// <summary>
        /// Draw fancy lines
        /// </summary>
        public override void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, settings.HideMaxDistance);
            Gizmos.DrawSphere(_targetPos, .5f);
        }
    }
}