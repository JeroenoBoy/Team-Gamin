using UnityEngine;

namespace UI
{
    public class PanelOpener : MonoBehaviour
    {
        [Header("Panels")]
        public GameObject StatsPanel;
        public GameObject BehaviourPanel;

        private Animator _animA;
        private Animator _animB;
        private static readonly int _stats = Animator.StringToHash("stats");
        private static readonly int _behaviour = Animator.StringToHash("behaviour");


        private void Start()
        {
            _animA = StatsPanel.GetComponent<Animator>();
            _animB = BehaviourPanel.GetComponent<Animator>();
        }

        
        /// <summary>
        /// function to open the stat point panel
        /// </summary>
        public void OpenStatsPanel()
        {
            if (_animA != null)
            {
                //reference to the Animator parameter
                bool isOpen = _animA.GetBool(_stats);

                _animA.SetBool(_stats, !isOpen);
            }
        }

        
        /// <summary>
        /// function to open the behaviour select panel
        /// </summary>
        public void OpenBehaviourPanel()
        {
            if (_animB != null)
            {
                //reference to the Animator parameter
                bool isOpen = _animB.GetBool(_behaviour);

                _animB.SetBool(_behaviour, !isOpen);
            }
        }
    }
}
