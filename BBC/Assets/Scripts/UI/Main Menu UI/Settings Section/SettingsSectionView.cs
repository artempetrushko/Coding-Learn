using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public enum SettingViewType
    {
        Switches,
        Slider
    }

    public class SettingsSectionView : MonoBehaviour
    {
        [SerializeField]
        private SliderOptionView sliderOptionViewPrefab;
        [SerializeField]
        private SwitchesOptionView switchesOptionViewPrefab;
        [SerializeField]
        private GameObject settingsContainer;
        [Space, SerializeField]
        private SettingsSectionAnimator animator;

        public IEnumerator ChangeVisibility_COR(bool isVisible)
        {
            yield return StartCoroutine(animator.ChangeVisibility_COR(isVisible));
        }

        public SettingsOptionView CreateOptionView(string localizedTitleReference, SettingViewType viewType)
        {
            SettingsOptionView optionView = (SettingsOptionView)Instantiate(viewType switch
            {
                SettingViewType.Switches => switchesOptionViewPrefab,
                SettingViewType.Slider => sliderOptionViewPrefab
            }, settingsContainer.transform);
            optionView.SetTitleReference(localizedTitleReference);
            return optionView;
        }

        /*public SliderOptionView CreateSliderOption() => Instantiate(sliderOptionViewPrefab, settingsContainer.transform);

        public SwitchesOptionView CreateSwitchesOption() => Instantiate(switchesOptionViewPrefab, settingsContainer.transform);

        public void CreateSliderOption(string optionTitle, UnityAction<float> sliderValueChangedAction)
        {
            var sliderOptionView = Instantiate(sliderOptionViewPrefab, settingsContainer.transform);
            sliderOptionView.SetParams(optionTitle, sliderValueChangedAction);
        }

        public void CreateSwitchesOption(string optionTitle, UnityAction previousValueButtonPressedAction, UnityAction nextValueButtonPressedAction)
        {
            var switchesOption = Instantiate(switchesOptionViewPrefab, settingsContainer.transform);
            switchesOption.SetParams(optionTitle, previousValueButtonPressedAction, nextValueButtonPressedAction);
        }*/
    }
}
