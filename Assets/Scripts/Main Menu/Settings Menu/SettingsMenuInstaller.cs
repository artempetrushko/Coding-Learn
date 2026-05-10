using UnityEngine;
using Zenject;

namespace MainMenu
{
    public class SettingsMenuInstaller : MonoInstaller
    {
        [SerializeField] private SettingsMenuView _view;

        public override void InstallBindings()
        {
            Container.Bind<SettingsMenuPresenter>().AsSingle().NonLazy();
            Container.Bind<SettingsMenuView>().FromInstance(_view).AsSingle().NonLazy();
        }
    }
}
