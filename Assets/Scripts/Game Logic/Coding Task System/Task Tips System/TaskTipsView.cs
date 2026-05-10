using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    public class TaskTipsView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [Space]
        [SerializeField] private TMP_Text _tipText;
        [SerializeField] private TMP_Text _tipStatusText;
        [SerializeField] private Button _showTipButton;
        [SerializeField] private Button _skipTaskButton;
        [SerializeField] private TMP_Text _skipTaskButtonText;       
        [SerializeField] private GameObject _tipFiller;

        public CanvasGroup CanvasGroup => _canvasGroup;
        public Button ShowTipButton => _showTipButton;
        public Button SkipTaskButton => _skipTaskButton;

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);

        public void SetTipText(string text) => _tipText.text = text;

        public void AddTipText(string text) => _tipText.text += text;

        public void SetTipStatusText(string text) => _tipStatusText.text = text;

        public void SetTipFillerActive(bool isActive) => _tipFiller.SetActive(isActive);

        public void SetSkipTaskButtonLabelText(string text) => _skipTaskButtonText.text = text;
    }
}
