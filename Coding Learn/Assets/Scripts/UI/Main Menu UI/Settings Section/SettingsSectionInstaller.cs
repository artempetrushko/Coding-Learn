using UnityEngine;
using Zenject;

namespace Scripts
{
    public class SettingsSectionInstaller : MonoInstaller
    {
        [SerializeField] private SettingsSectionView _settingsSectionView;
        [SerializeField] private SliderOptionView _sliderOptionViewPrefab;
        [SerializeField] private SwitchesOptionView _switchesOptionViewPrefab;

        public override void InstallBindings()
        {
            Container.Bind<SettingsSectionController>().AsSingle().NonLazy();
            Container.Bind<SettingsSectionView>().FromInstance(_settingsSectionView).AsSingle().NonLazy();
            Container.Bind<SliderOptionView>().FromComponentInNewPrefab(_sliderOptionViewPrefab).AsSingle().NonLazy();
            Container.Bind<SwitchesOptionView>().FromComponentInNewPrefab(_switchesOptionViewPrefab).AsSingle().NonLazy();
        }
    }
}
