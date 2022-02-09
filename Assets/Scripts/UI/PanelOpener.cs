using UnityEngine;
public class PanelOpener : MonoBehaviour
{
    public GameObject StatsPanel;
    public GameObject BehaviourPanel;
    Animator animA => StatsPanel.GetComponent<Animator>();
    Animator animB => BehaviourPanel.GetComponent<Animator>();


    public void OpenStatsPanel()
    {
        if (animA != null)
        {
            bool isOpen = animA.GetBool("stats");

            animA.SetBool("stats", !isOpen);
        }
    }

    public void OpenBehaviourPanel()
    {
        if (animB != null)
        {
            bool isOpen = animB.GetBool("behaviour");

            animB.SetBool("behaviour", !isOpen);
        }
    }
}
