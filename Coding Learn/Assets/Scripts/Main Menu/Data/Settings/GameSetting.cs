using System;
using System.Collections;
using System.Collections.Generic;

namespace Scripts
{
    public class GameSetting
    {
        private SettingsOptionView optionView;
        private List<string> formattedSettingValues = new List<string>();
        private int currentValueOrderNumber;
        private Func<string> getCurrentValueFunction;
        private Action<string> applyValueAction;

        public GameSetting(SettingsOptionView view, List<string> formattedSettingValues, Func<string> getCurrentValueFunction, Action<string> applyValueAction)
        {
            optionView = view;
            this.formattedSettingValues = formattedSettingValues;
            this.getCurrentValueFunction = getCurrentValueFunction;
            this.applyValueAction = applyValueAction;

            optionView.SetValueChangedAction(optionView switch
            {
                SwitchesOptionView => SetNeighboringValue,
                SliderOptionView => SetNewValue
            });
            SetCurrentValue();
            if (optionView is SliderOptionView sliderOptionView)
            {
                sliderOptionView.SetDefaultSliderValues(int.Parse(formattedSettingValues[0]), int.Parse(formattedSettingValues[^1]), 
                    int.Parse(formattedSettingValues[currentValueOrderNumber - 1]));
            }
        }

        public void SetCurrentValue()
        {
            var formattedCurrentValue = getCurrentValueFunction();
            SetNewValue(formattedSettingValues.IndexOf(formattedCurrentValue) + 1);
        }

        public void ApplyValue() => applyValueAction.Invoke(formattedSettingValues[currentValueOrderNumber - 1]);

        private void SetNewValue(int valueOrderNumber)
        {
            currentValueOrderNumber = valueOrderNumber;
            optionView.SetOptionValue(formattedSettingValues[currentValueOrderNumber - 1]);
        }

        private void SetNeighboringValue(int orderNumberOffset)
        {
            if (currentValueOrderNumber + orderNumberOffset > formattedSettingValues.Count)
            {
                SetNewValue(1);
            }
            else if (currentValueOrderNumber + orderNumberOffset <= 0)
            {
                SetNewValue(formattedSettingValues.Count);
            }
            else
            {
                SetNewValue(currentValueOrderNumber + orderNumberOffset);
            }  
        }
    }
}
