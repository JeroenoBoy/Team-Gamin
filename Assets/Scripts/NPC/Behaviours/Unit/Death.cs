using Controllers;
using Effects;
using NPC.Brains;
using NPC.Utility;
using UnityEngine;

namespace NPC.Behaviours.Unit
{
    public class Death : StateMachineBehaviour
    {
        [SerializeField] private float     _dieAfter;
        [SerializeField] private DeathAnim _deathAnim;
        [SerializeField] private bool _destroy = false;

        private float _dieAt;
        private bool  _died;
        
        
        public override void OnStateEnter(Animator anim, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _dieAt = Time.time + _dieAfter;
            _died = false;
            
            var transform = anim.transform;
            if(anim.TryGetComponent(out MovementController ctrl))
                ctrl.canMove = false;

            if (_deathAnim)
                Instantiate(_deathAnim.gameObject, transform.position, transform.rotation)
                    .GetComponent<DeathAnim>().team = transform.GetComponent<UnitBrain>().team;
        }
        

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(_died || Time.time < _dieAt) return;
            
            _died = true;
            if(_destroy) Destroy(animator.gameObject);
            else animator.gameObject.SetActive(false);
        }


        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(animator.transform.TryGetComponent(out MovementController ctrl))
                ctrl.canMove = false;
        }
    }
}