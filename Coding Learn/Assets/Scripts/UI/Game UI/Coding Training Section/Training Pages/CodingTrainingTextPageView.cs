using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class CodingTrainingTextPageView : MonoBehaviour
    {
        [SerializeField] protected TMP_Text _trainingText;
        [SerializeField] protected Scrollbar _trainingTextSlider;

        public void SetTrainingText(string text) => _trainingText.text = text;

        public void SetSliderValue(float value) => _trainingTextSlider.value = value;
    }
}
