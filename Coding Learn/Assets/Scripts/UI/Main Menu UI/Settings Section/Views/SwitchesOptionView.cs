using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class SwitchesOptionView : SettingsOptionView
    {
        [SerializeField] private Button _previousValueButton;
        [SerializeField] private Button _nextValueButton;

        public override void SetValueChangedAction(Action<int> optionValueChangedAction)
        {
            _previousValueButton.onClick.AddListener(() => optionValueChangedAction(-1));
            _nextValueButton.onClick.AddListener(() => optionValueChangedAction(1));
        }
    }
}
