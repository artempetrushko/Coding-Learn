using TMPro;
using UnityEngine;

namespace Scripts
{
    public class TaskStatsView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _taskTitleText;
        [SerializeField] private TMP_Text _starsCounterText;

        public void SetTaskTitleText(string text) => _taskTitleText.text = text;

        public void SetStarsCounterText(string text) => _starsCounterText.text = text;
    }
}
