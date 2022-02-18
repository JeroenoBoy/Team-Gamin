using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class StatPoints : MonoBehaviour
    {
        public statpointsdata[] data;

        [Header("Values")]
        public float statPoints = 100;
        [SerializeField]private float usedStatPoints;
        [SerializeField]private float multiplier = 5;

        [Header("Text")]
        [SerializeField] private TextMeshProUGUI statPointsText;
        [SerializeField] private TextMeshProUGUI currentMultiplier;

        private void Start()
        {
            //adds listeners to + and - buttons in the stats panel
            for (int i = 0; i < data.Length; i++)
            {
                data[i].plus.onClick.AddListener(IncreasePoints(data[i]));
                data[i].minus.onClick.AddListener(DecreasePoints(data[i]));
            }
        }
        private void Update()
        {
            //updates the text
            statPointsText.text = string.Format("StatPoints : {0}/100", usedStatPoints);
            currentMultiplier.text = string.Format("Current Multiplier : {0}", multiplier + "x");
        }

        /// <summary>
        /// function to increase statpoints depending on what button you click
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public UnityAction IncreasePoints(statpointsdata data)
        {
            return () =>
            {
                //clamps the stat points
                var newStatPoints = Mathf.Clamp(usedStatPoints + multiplier, 0, statPoints);
                var changed = newStatPoints - usedStatPoints;
                usedStatPoints = newStatPoints;

                //updates the text
                data.slider.value = data.value += changed;
                data.slidersText.text = string.Format(data.slidersName + " : {0}", data.slider.value);

            };
        }

        /// <summary>
        /// function to decrease statpoints depending on what button you click
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public UnityAction DecreasePoints(statpointsdata data)
        {
            return () =>
            {
                //clamps the stat points
                var newStatPoints = Mathf.Clamp(data.value - multiplier, 0, statPoints);
                var changed = newStatPoints - data.value;
                usedStatPoints += changed;

                //updates the text
                data.slider.value = data.value += changed;
                data.slidersText.text = string.Format(data.slidersName + " : {0}", data.slider.value);
            };
        }

        /// <summary>
        /// function to change the current multiplier with a parameter, call this function on a button
        /// </summary>
        /// <param name="amount"></param>
        public void ChangeMultiplier(float amount)
        {
            multiplier = amount;
        }
    }

    [System.Serializable]
    public class statpointsdata //class to store slider data
    {
        public string slidersName;
        public TextMeshProUGUI slidersText;
        public Slider slider;
        public float value;
        public Button minus;
        public Button plus;
    }
}