using UnityEngine;

public class ActivateTraitsUI : MonoBehaviour
{
    public GameObject TraitsPanel;

    public void OnMouseDown()
    {
        TraitsPanel.SetActive(true);
    }
}
