using Cysharp.Threading.Tasks;
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

        public async UniTask ChangeVisibilityAsync(bool isVisible) => await animator.ChangeVisibilityAsync(isVisible);

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
