using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public abstract class SettingsOptionView : MonoBehaviour
    {
        [SerializeField]
        protected TMP_Text optionTitleText;
        [SerializeField]
        protected TMP_Text optionValueText;

        //public abstract void SetParams(string optionTitle, UnityAction<float> optionValueChangedAction);

        public void SetOptionValue(string optionValue) => optionValueText.text = optionValue;
    }
}
