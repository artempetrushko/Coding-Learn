using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scripts
{
    public class SliderOptionView : SettingsOptionView
    {
        [SerializeField]
        private Slider slider;

        public void SetParams(string optionTitle, UnityAction<float> optionValueChangedAction)
        {
            optionTitleText.text = optionTitle;
            slider.onValueChanged.AddListener(optionValueChangedAction);
        }
    }
}
