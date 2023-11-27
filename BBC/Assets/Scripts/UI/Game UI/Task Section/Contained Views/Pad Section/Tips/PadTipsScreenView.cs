using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

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

        public void Show() => StartCoroutine(animator.ChangeViewVisibility_COR(gameObject, true));

        public void Hide() => StartCoroutine(animator.ChangeViewVisibility_COR(gameObject, false));

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

        public void SetSkipTaskButtonLabelText(string text) => skipTaskButton.GetComponentInChildren<TMP_Text>().text = text;

        public void SetTipStatusText(string status) => tipStatusText.text = status;

        public void ClearTipText()
        {
            tipText.text = "";
            tipFiller.gameObject.SetActive(true);
        }
    }
}
