using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class PadTipsScreenView : PadModalWindow
    {
        [SerializeField] 
        private Button showTipButton;
        [SerializeField] 
        private Button skipTaskButton;
        [SerializeField] 
        private TMP_Text tipText;
        [SerializeField] 
        private TMP_Text tipStatusText;
        [SerializeField] 
        private GameObject tipFiller;

        public void AddNewTipText(string tip)
        {
            if (tipFiller.activeInHierarchy)
            {
                tipFiller.SetActive(false);
            }
            tipText.text += $" - {tip}\n";
        }

        public void SetShowTipButtonState(bool isInteractable) => showTipButton.interactable = isInteractable;

        public void SetSkipTaskButtonState(bool isInteractable) => skipTaskButton.interactable = isInteractable;

        public void SetTipStatusText(string statusText) => tipStatusText.text = statusText;

        public void SetSkipTaskButtonLabelText(string labelText) => skipTaskButton.GetComponentInChildren<TMP_Text>().text = labelText;

        public void ClearTipText()
        {
            tipText.text = "";
            tipFiller.SetActive(true);
        }
    }
}
