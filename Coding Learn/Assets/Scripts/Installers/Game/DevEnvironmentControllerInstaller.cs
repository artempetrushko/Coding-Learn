using UnityEngine;
using Zenject;

namespace Scripts
{
    public class DevEnvironmentControllerInstaller : MonoInstaller
    {
        [SerializeField]
        private PadDevEnvironmentView padDevEnvironmentView;

        public override void InstallBindings()
        {
            Container.Bind<PadDevEnvironmentView>().FromInstance(padDevEnvironmentView).AsSingle().NonLazy();
            Container.Bind<DevEnvironmentController>().ToSelf().AsSingle().NonLazy();
        }
    }
}