using UnityEngine;
using Zenject;

namespace Scripts
{
    public class LevelLoadingControllerInstaller : MonoInstaller
    {
        [SerializeField]
        private LoadingScreenView loadingScreenView;

        public override void InstallBindings()
        {
            Container.Bind<LoadingScreenView>().FromInstance(loadingScreenView).AsSingle().NonLazy();
            Container.Bind<LevelLoadingManager>().ToSelf().AsSingle().NonLazy();
        }
    }
}