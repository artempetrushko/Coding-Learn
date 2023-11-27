using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class SliderOptionView : SettingsOptionView
    {
        [SerializeField]
        private Slider slider;

        public override void SetValueChangedAction(Action<int> optionValueChangedAction)
        {
            slider.onValueChanged.AddListener((value) => optionValueChangedAction((int)value));
        }
    }
}
