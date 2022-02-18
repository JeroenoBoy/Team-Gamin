using UnityEngine;
public class PanelOpener : MonoBehaviour
{
    [Header("Panels")]
    public GameObject StatsPanel;
    public GameObject BehaviourPanel;

    private Animator animA;
    private Animator animB;

    private void Start()
    {
        animA = StatsPanel.GetComponent<Animator>();

        animB = BehaviourPanel.GetComponent<Animator>();
    }

    /// <summary>
    /// funtion to open the statpoint panel
    /// </summary>
    public void OpenStatsPanel()
    {
        if (animA != null)
        {
            //reference to the Animator parameter
            bool isOpen = animA.GetBool("stats");

            animA.SetBool("stats", !isOpen);
        }
    }

    /// <summary>
    /// function to open the behaviour select panel
    /// </summary>
    public void OpenBehaviourPanel()
    {
        if (animB != null)
        {
            //reference to the Animator parameter
            bool isOpen = animB.GetBool("behaviour");

            animB.SetBool("behaviour", !isOpen);
        }
    }
}
