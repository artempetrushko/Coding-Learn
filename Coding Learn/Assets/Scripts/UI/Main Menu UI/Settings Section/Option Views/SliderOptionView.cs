using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class SliderOptionView : SettingsOptionView
    {
        [SerializeField] private Slider slider;

        public override void SetValueChangedAction(Action<int> optionValueChangedAction)
        {
            slider.onValueChanged.AddListener((value) => optionValueChangedAction((int)value));
        }

        public void SetSliderMinValue(int minValue) => slider.minValue = minValue;

        public void SetSliderMaxValue(int maxValue) => slider.maxValue = maxValue;

        public void SetSliderCurrentValue(int value) => slider.value = value;
    }
}
