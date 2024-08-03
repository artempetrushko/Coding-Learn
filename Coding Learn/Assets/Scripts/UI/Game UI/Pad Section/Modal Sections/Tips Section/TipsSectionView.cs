using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class TipsSectionView : PadModalWindow
    {
        [SerializeField] private Button _showTipButton;
        [SerializeField] private Button _skipTaskButton;
        [SerializeField] private TMP_Text _tipText;
        [SerializeField] private TMP_Text _tipStatusText;
        [SerializeField] private GameObject _tipFiller;

        public void SetTipText(string text) => _tipText.text = text;

        public void AddTipText(string text) => _tipText.text += text;

        public void SetTipStatusText(string text) => _tipStatusText.text = text;

        public void SetTipFillerActive(bool isActive) => _tipFiller.SetActive(isActive);

        public void SetShowTipButtonInteractable(bool isInteractable) => _showTipButton.interactable = isInteractable;

        public void SetSkipTaskButtonInteractable(bool isInteractable) => _skipTaskButton.interactable = isInteractable;

        public void SetSkipTaskButtonLabelText(string text) => _skipTaskButton.GetComponentInChildren<TMP_Text>().text = text;
    }
}
