using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Tables;

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

        public SettingsOptionView CreateOptionView((TableReference table, TableEntryReference entry) localizedString, SettingViewType viewType)
        {
            var optionView = (SettingsOptionView)Instantiate(viewType switch
            {
                SettingViewType.Switches => switchesOptionViewPrefab,
                SettingViewType.Slider => sliderOptionViewPrefab
            }, settingsContainer.transform);
            optionView.SetTitleReference(localizedString);
            return optionView;
        }
    }
}
