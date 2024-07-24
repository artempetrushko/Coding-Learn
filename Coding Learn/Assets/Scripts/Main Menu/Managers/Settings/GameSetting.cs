using System.Collections.Generic;

namespace Scripts
{
    public abstract class GameSetting
    {
        private SettingsOptionView optionView;
        private List<string> formattedSettingValues = new();
        private int currentValueOrderNumber;

        protected string CurrentFormattedValue => formattedSettingValues[currentValueOrderNumber - 1];

        public GameSetting(SettingsOptionView view)
        {
            optionView = view;

            optionView.SetValueChangedAction(optionView switch
            {
                SwitchesOptionView => SetNeighboringValue,
                SliderOptionView => SetNewValue
            });
            SetCurrentValue();
            if (optionView is SliderOptionView sliderOptionView)
            {
                sliderOptionView.SetSliderMinValue(int.Parse(formattedSettingValues[0]));
                sliderOptionView.SetSliderMaxValue(int.Parse(formattedSettingValues[^1]));
                sliderOptionView.SetSliderCurrentValue(int.Parse(formattedSettingValues[currentValueOrderNumber - 1]));
            }
        }

        public abstract void ApplyValue();

        protected abstract string GetCurrentValue();

        protected abstract List<string> GetFormattedSettingValues();

        public void SetCurrentValue()
        {
            var formattedCurrentValue = GetCurrentValue();
            SetNewValue(formattedSettingValues.IndexOf(formattedCurrentValue) + 1);
        }

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
