using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scripts
{
    public class SwitchesOptionView : SettingsOptionView
    {
        [SerializeField]
        private Button previousValueButton;
        [SerializeField]
        private Button nextValueButton;

        public void SetParams(string optionTitle, UnityAction previousValueButtonPressedAction, UnityAction nextValueButtonPressedAction)
        {
            optionTitleText.text = optionTitle;
            previousValueButton.onClick.AddListener(previousValueButtonPressedAction);
            nextValueButton.onClick.AddListener(nextValueButtonPressedAction);
        }
    }
}
