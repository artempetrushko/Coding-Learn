using MainMenu;
using UnityEngine;
using Zenject;

namespace UI.MainMenu
{
    public class LevelsMenuInstaller : MonoInstaller
    {
        [SerializeField] private LevelsMenuView _view;
        [SerializeField] private LevelsMenuConfig _levelsSectionConfig;

        public override void InstallBindings()
        {
            Container.Bind<LevelsMenuPresenter>().AsSingle().NonLazy();
            Container.Bind<LevelsMenuView>().FromInstance(_view).AsSingle().NonLazy();
            Container.Bind<LevelsMenuConfig>().FromScriptableObject(_levelsSectionConfig).AsSingle().NonLazy();
        }
    }
}
