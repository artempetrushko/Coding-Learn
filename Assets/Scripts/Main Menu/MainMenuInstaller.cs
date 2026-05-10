using MainMenu;
using UnityEngine;
using Zenject;

namespace UI.MainMenu
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private MainMenuView _view;

        public override void InstallBindings()
        {
            Container.Bind<MainMenuPresenter>().AsSingle().NonLazy();
            Container.Bind<MainMenuView>().FromInstance(_view).AsSingle().NonLazy();
        }
    }
}
