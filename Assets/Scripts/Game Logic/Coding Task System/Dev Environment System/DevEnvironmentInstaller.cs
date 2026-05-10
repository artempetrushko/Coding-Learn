using UnityEngine;
using Zenject;

namespace GameLogic
{
    public class DevEnvironmentInstaller : MonoInstaller
    {
        [SerializeField] private DevEnvironmentView _view;

        public override void InstallBindings()
        {
            Container.Bind<DevEnvironmentPresenter>().AsSingle().NonLazy();
            Container.Bind<DevEnvironmentView>().FromInstance(_view).AsSingle().NonLazy();
        }
    }
}
