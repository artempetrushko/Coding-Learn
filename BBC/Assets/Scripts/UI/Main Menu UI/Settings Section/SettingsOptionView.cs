using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace Scripts
{
    public abstract class SettingsOptionView : MonoBehaviour
    {
        [SerializeField]
        protected TMP_Text optionTitleText;
        [SerializeField]
        protected TMP_Text optionValueText;

        public abstract void SetValueChangedAction(Action<int> optionValueChangedAction);

        public void SetTitleReference(string localizedTitleReference)
        {
            optionTitleText.GetComponent<LocalizeStringEvent>().StringReference.SetReference("Main Menu UI", localizedTitleReference);
        }

        public void SetOptionValue(string optionValue) => optionValueText.text = optionValue;
    }
}
