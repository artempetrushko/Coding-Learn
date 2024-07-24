using Cysharp.Threading.Tasks;
using DG.Tweening;
using Zenject;

namespace Scripts
{
    public class SettingsSectionController 
    {
        private const float VISIBILITY_CHANGING_TIME = 0.75f;

        private DiContainer _container;
        private SettingsSectionView _settingsSectionView;
        private SliderOptionView _sliderOptionViewPrefab;
        private SwitchesOptionView _switchesOptionViewPrefab;

        public SettingsSectionController(DiContainer diContainer, SettingsSectionView settingsSectionView, SliderOptionView sliderOptionViewPrefab, SwitchesOptionView switchesOptionViewPrefab)
        {
            _container = diContainer;
            _settingsSectionView = settingsSectionView;
            _sliderOptionViewPrefab = sliderOptionViewPrefab;
            _switchesOptionViewPrefab = switchesOptionViewPrefab;
        }

        public SettingsOptionView CreateOptionView(SettingData settingData)
        {
            SettingsOptionView optionView = _container.InstantiatePrefab(settingData.ViewType switch
            {
                SettingViewType.Switches => _switchesOptionViewPrefab,
                SettingViewType.Slider => _sliderOptionViewPrefab
            }, _settingsSectionView.SettingViewsContainer.transform).GetComponent<SettingsOptionView>();
            optionView.SetOptionTitle(settingData.Name.GetLocalizedString());
            return optionView;
        }

        public async UniTask ChangeVisibilityAsync(bool isVisible)
        {
            await _settingsSectionView.transform
                .DOLocalMoveY(isVisible ? 0 : _settingsSectionView.GetSectionHeight(), VISIBILITY_CHANGING_TIME)
                .AsyncWaitForCompletion();
        }
    }
}
