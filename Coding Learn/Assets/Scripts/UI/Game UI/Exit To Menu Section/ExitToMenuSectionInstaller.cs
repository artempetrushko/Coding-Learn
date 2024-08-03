using UnityEngine;
using Zenject;

namespace Scripts
{
    public class ExitToMenuSectionInstaller : MonoInstaller
    {
        [SerializeField] private ExitToMenuSectionView _exitToMenuSectionView;

        public override void InstallBindings()
        {
            Container.Bind<ExitToMenuSectionController>().AsSingle().NonLazy();
            Container.Bind<ExitToMenuSectionView>().FromInstance(_exitToMenuSectionView).AsSingle().NonLazy();
        }
    }
}
