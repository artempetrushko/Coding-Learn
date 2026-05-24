using MainMenu;
using UI.MainMenu;
using UnityEngine;
using Zenject;

namespace Scripts.Bootstrap.MainMenu
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private MainMenuView _mainMenuView;
        [SerializeField] private LevelsMenuView _levelsMenuview;
        [SerializeField] private LevelsMenuConfig _levelsSectionConfig;
        [SerializeField] private SettingsMenuView _settingsMenuView;
        [SerializeField] private StatisticsMenuView _statisticsMenuView;

        public override void InstallBindings()
        {
            Container.Bind<MainMenuPresenter>().AsSingle().NonLazy();
            Container.Bind<MainMenuView>().FromInstance(_mainMenuView).AsSingle().NonLazy();

            Container.Bind<LevelsMenuPresenter>().AsSingle().NonLazy();
            Container.Bind<LevelsMenuView>().FromInstance(_levelsMenuview).AsSingle().NonLazy();
            Container.Bind<LevelsMenuConfig>().FromScriptableObject(_levelsSectionConfig).AsSingle().NonLazy();

            Container.Bind<SettingsMenuPresenter>().AsSingle().NonLazy();
            Container.Bind<SettingsMenuView>().FromInstance(_settingsMenuView).AsSingle().NonLazy();

            Container.Bind<StatisticsMenuPresenter>().AsSingle().NonLazy();
            Container.Bind<StatisticsMenuView>().FromInstance(_statisticsMenuView).AsSingle().NonLazy();
        }
    }
}
