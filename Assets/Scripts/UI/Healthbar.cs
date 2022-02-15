using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;
    [SerializeField] private Canvas canvas;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        slider.value = health;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void OnEnable()
    {
        TurnOffHealthBars.instance.canvasses.Add(canvas);
        canvas.enabled = TurnOffHealthBars.instance.toggled;
    }

    public void OnDisable()
    {
        TurnOffHealthBars.instance.canvasses.Remove(canvas);
    }
}
