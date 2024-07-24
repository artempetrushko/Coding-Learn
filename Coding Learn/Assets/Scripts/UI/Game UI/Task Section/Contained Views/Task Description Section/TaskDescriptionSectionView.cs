using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class TaskDescriptionSectionView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _taskTitleText;
        [SerializeField] private TMP_Text _taskDescriptionText;
        [SerializeField] private Scrollbar _scrollbar;

        public void SetTaskTitleText(string text) => _taskTitleText.text = text;

        public void SetTaskDescriptionText(string text) => _taskDescriptionText.text = text;

        public void SetScrollbarValue(float value) => _scrollbar.value = value;
    }
}
