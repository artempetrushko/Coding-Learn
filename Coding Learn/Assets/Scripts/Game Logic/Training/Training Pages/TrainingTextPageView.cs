using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class TrainingTextPageView : MonoBehaviour
    {
        [SerializeField] protected TMP_Text _trainingText;
        [SerializeField] protected Scrollbar _trainingTextScrollbar;

        public void SetTrainingText(string text) => _trainingText.text = text;

        public void SetTrainingTextScrollbarValue(float value) => _trainingTextScrollbar.value = value;
    }
}
