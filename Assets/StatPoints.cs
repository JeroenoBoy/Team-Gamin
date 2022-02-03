using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatPoints : MonoBehaviour
{
    [SerializeField] private float statPoints = 100;
    [SerializeField] private TextMeshProUGUI statPointText;

    [Header("stat points")]
    [SerializeField]private float atk;
    [SerializeField]private float atk_speed;
    [SerializeField]private float move_speed;
    [SerializeField]private float sight_range;
    [SerializeField]private float defence;

    private float sum;

    [Header("Sliders")]
    [SerializeField] private data[] dataArray;

    void Update()
    {
        statPointText.text = string.Format("StatPoints : {0}", statPoints);

        //check if all stat points are used up    
        if (statPoints <= 0)
        {
            Debug.Log("Insuffient statpoints");
            //make it so you cant use anymore statpoints but can decrease other sliders to get statpoints back
        }
    }

    private void CalculateSum()
    {
        var unusedStatPoints = 0f;

        for (int i = 0; i < dataArray.Length; i++)
        {
            unusedStatPoints += dataArray[i].slider.value;
        }

        sum = 100 - unusedStatPoints;
    }

    public void OnValueChange()
    {
        CalculateSum();

        statPoints = sum;

        for (int i = 0; i < dataArray.Length; i++)
        {
            dataArray[i].slidersText.text = string.Format(dataArray[i].slidersName + " : {0}", dataArray[i].slider.value);
        }

        //change max value to how many statpoints are left
    }

    /*public void SetAttack()
    {
        float input = dataArray[0].slider.value - atk;
        float atksum = statPoints;
        atksum = atksum / (atksum + input);
        atk = (atk + input) * atksum; 
        atk_speed *= atksum;
        move_speed *= atksum;
        sight_range *= atksum;
        defence *= atksum;
    }*/
}

[System.Serializable]
public struct data
{
    public string slidersName;
    public TextMeshProUGUI slidersText;
    public Slider slider;
}
