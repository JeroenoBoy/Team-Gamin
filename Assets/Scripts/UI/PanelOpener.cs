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

        
        private void Start()
        {
            _animA = StatsPanel.GetComponent<Animator>();
            _animB = BehaviourPanel.GetComponent<Animator>();
        }

        
        /// <summary>
        /// funtion to open the statpoint panel
        /// </summary>
        public void OpenStatsPanel()
        {
            if (_animA != null)
            {
                //reference to the Animator parameter
                bool isOpen = _animA.GetBool("stats");

                _animA.SetBool("stats", !isOpen);
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
                bool isOpen = _animB.GetBool("behaviour");

                _animB.SetBool("behaviour", !isOpen);
            }
        }
    }
}
