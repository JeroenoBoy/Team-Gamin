using UnityEngine;
public class PanelOpener : MonoBehaviour
{
    public GameObject panel;
    Animator anim => panel.GetComponent<Animator>();

    public void OpenPanel()
    {
        if (anim != null)
        {
            bool isOpen = anim.GetBool("open");

            anim.SetBool("open", !isOpen);
        }
    }
}
