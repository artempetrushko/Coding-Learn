using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public enum SettingType
    {
        Resolution,
        ScreenMode,
        GraphicsQuality,
        Language,
        SoundsVolume,
        MusicVolume
    }

    public class GameSetting
    {
        private SettingsOptionView optionView;
        private List<string> formattedSettingValues = new List<string>();
        private int currentValueOrderNumber;
        private Action<string> applySettingValueAction;

        public GameSetting(SettingsOptionView view, List<string> formattedSettingValues, Action<string> applySettingValueAction)
        {
            optionView = view;
            this.formattedSettingValues = formattedSettingValues;
            this.applySettingValueAction = applySettingValueAction;

            optionView.SetValueChangedAction(optionView switch
            {
                SwitchesOptionView => SetNeighboringValue,
                SliderOptionView => SetNewValue
            });
        }

        public void ApplySetting() => applySettingValueAction.Invoke(formattedSettingValues[currentValueOrderNumber - 1]);

        private void SetNewValue(int valueOrderNumber)
        {
            currentValueOrderNumber = valueOrderNumber;
            optionView.SetOptionValue(formattedSettingValues[currentValueOrderNumber - 1]);
        }

        private void SetNeighboringValue(int orderNumberOffset) => SetNewValue(currentValueOrderNumber + orderNumberOffset);
    }
}
