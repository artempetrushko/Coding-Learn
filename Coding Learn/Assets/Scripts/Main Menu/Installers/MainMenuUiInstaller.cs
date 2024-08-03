using UnityEngine;
using Zenject;

namespace Scripts
{
    public class MainMenuUiInstaller : MonoInstaller
    {
        [SerializeField] private LevelsSectionView _levelsSectionView;
        [SerializeField] private SettingsSectionView _settingsSectionView;
        [SerializeField] private StatsSectionView _statsSectionView;

        public override void InstallBindings()
        {
            Container.Bind<LevelsSectionController>().AsSingle().NonLazy();
            Container.Bind<LevelsSectionView>().FromInstance(_levelsSectionView).AsSingle().NonLazy();

            Container.Bind<SettingsSectionController>().AsSingle().NonLazy();
            Container.Bind<SettingsSectionView>().FromInstance(_settingsSectionView).AsSingle().NonLazy();

            Container.Bind<StatsSectionController>().AsSingle().NonLazy();
            Container.Bind<StatsSectionView>().FromInstance(_statsSectionView).AsSingle().NonLazy();
        }
    }
}