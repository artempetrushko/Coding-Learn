using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Components;

namespace Scripts
{
    public class PadTipsScreenView : MonoBehaviour
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
        private TMP_Text tipFiller;
        [Space, SerializeField]
        private PadViewsAnimator animator;

        public void ChangeVisibility(bool isVisible) => StartCoroutine(animator.ChangeViewVisibility_COR(gameObject, isVisible));

        public void AddNewTipText(string tip)
        {
            if (tipFiller.isActiveAndEnabled)
            {
                tipFiller.gameObject.SetActive(false);
            }
            tipText.text += string.Format(@" - {0}\n", tip);
        }

        public void SetShowTipButtonState(bool isInteractable) => showTipButton.interactable = isInteractable;

        public void SetSkipTaskButtonState(bool isInteractable) => skipTaskButton.interactable = isInteractable;

        public void SetSkipTaskButtonLabelText(string localizationTableReference, string localizedTextReference) 
            => skipTaskButton.GetComponentInChildren<LocalizeStringEvent>().StringReference.SetReference(localizationTableReference, localizedTextReference);

        public void SetSkipTaskButtonLabelTextWithTimer(string localizationTableReference, string localizedTextReference, string formattedTimerText)
        {
            var buttonTextReference = skipTaskButton.GetComponentInChildren<LocalizeStringEvent>().StringReference;
            buttonTextReference.SetReference(localizationTableReference, localizedTextReference);
            var buttonLocalizedText = buttonTextReference.GetLocalizedString(formattedTimerText);
            //Localization
        }

        public void SetTipStatusText(string localizationTableReference, string localizedTextReference)
            => tipStatusText.GetComponent<LocalizeStringEvent>().StringReference.SetReference(localizationTableReference, localizedTextReference);

        public void SetTipStatusTextWithTimer(string localizationTableReference, string localizedTextReference, string formattedTimerText)
        {
            
        }

        public void ClearTipText()
        {
            tipText.text = "";
            tipFiller.gameObject.SetActive(true);
        }
    }
}
