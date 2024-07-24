using System;
using TMPro;
using UnityEngine;

namespace Scripts
{
    public abstract class SettingsOptionView : MonoBehaviour
    {
        [SerializeField] protected TMP_Text _optionTitleText;
        [SerializeField] protected TMP_Text _optionValueText;

        public abstract void SetValueChangedAction(Action<int> optionValueChangedAction);

        public void SetOptionTitle(string optionTitle) => _optionTitleText.text = optionTitle;

        public void SetOptionValue(string optionValue) => _optionValueText.text = optionValue;
    }
}
