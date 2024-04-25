using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
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

        public void SetSkipTaskButtonLabelText(string localizationTableReference, string localizedTextReference) 
            => skipTaskButton.GetComponentInChildren<LocalizeStringEvent>().StringReference.SetReference(localizationTableReference, localizedTextReference);

        public void SetSkipTaskButtonLabelTextWithTimer(string localizationTableReference, string localizedTextReference, string formattedTimerText) 
            => SetLocalizedTextWithTimer(skipTaskButton.GetComponentInChildren<TMP_Text>(), localizationTableReference, localizedTextReference, formattedTimerText);

        public void SetTipStatusText(string localizationTableReference, string localizedTextReference)
            => tipStatusText.GetComponent<LocalizeStringEvent>().StringReference.SetReference(localizationTableReference, localizedTextReference);

        public void SetTipStatusTextWithTimer(string localizationTableReference, string localizedTextReference, string formattedTimerText) 
            => SetLocalizedTextWithTimer(tipStatusText, localizationTableReference, localizedTextReference, formattedTimerText);

        public void ClearTipText()
        {
            tipText.text = "";
            tipFiller.SetActive(true);
        }

        private void SetLocalizedTextWithTimer(TMP_Text textField, string localizationTableReference, string localizedTextReference, string formattedTimerText)
        {
            var localizedString = textField.GetComponent<LocalizeStringEvent>().StringReference;
            localizedString.SetReference(localizationTableReference, localizedTextReference);
            textField.text = localizedString.GetLocalizedString(formattedTimerText);
        }
    }
}
