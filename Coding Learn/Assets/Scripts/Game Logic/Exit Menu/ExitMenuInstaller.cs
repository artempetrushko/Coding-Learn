using UnityEngine;
using Zenject;

namespace UI.Game
{
    public class ExitMenuInstaller : MonoInstaller
    {
        [SerializeField] private ExitMenuView _view;

        public override void InstallBindings()
        {
            Container.Bind<ExitMenuPresenter>().AsSingle().NonLazy();
            Container.Bind<ExitMenuView>().FromInstance(_view).AsSingle().NonLazy();
        }
    }
}
