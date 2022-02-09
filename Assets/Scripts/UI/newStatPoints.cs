using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using TMPro;


public class newStatPoints : MonoBehaviour
{
    public statpointsdata[] data;

    [Header("Values")]
    [SerializeField]private float statPoints = 100;
    [SerializeField]private float usedStatPoints;
    [SerializeField]private float multiplier = 5;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI statPointsText;
    [SerializeField] private TextMeshProUGUI currentMultiplier;


    private void Start()
    {
        for (int i = 0; i < data.Length; i++)
        {
            data[i].plus.onClick.AddListener(IncreasePoints(data[i]));
            data[i].minus.onClick.AddListener(DecreasePoints(data[i]));
        }
    }
    private void Update()
    {
        statPointsText.text = string.Format("StatPoints : {0}/100", usedStatPoints);
        currentMultiplier.text = string.Format("Current Multiplier : {0}", multiplier + "x");
    }
    public UnityAction IncreasePoints(statpointsdata data)
    {
        return () =>
         {
             var newStatPoints = Mathf.Clamp(usedStatPoints + multiplier, 0, statPoints);
             var changed = newStatPoints - usedStatPoints;
             usedStatPoints = newStatPoints;

             data.slider.value = data.value += changed;
             data.slidersText.text = string.Format(data.slidersName + " : {0}", data.slider.value);

         };
    }
    public UnityAction DecreasePoints(statpointsdata data)
    {
        return () =>
        {
            var newStatPoints = Mathf.Clamp(usedStatPoints - multiplier, 0, statPoints);
            var changed = newStatPoints - usedStatPoints;
            usedStatPoints = newStatPoints;

            data.slider.value = data.value += changed;
            data.slidersText.text = string.Format(data.slidersName + " : {0}", data.slider.value);
        };
    }

    public void ChangeMultiplier(float amount)
    {
        multiplier = amount;
    }
}

[System.Serializable]
public class statpointsdata //struct to store slider data
{
    public string slidersName;
    public TextMeshProUGUI slidersText;
    public Slider slider;
    public float value;
    public Button minus;
    public Button plus;
}