using UnityEngine;
using Zenject;

namespace Scripts
{
    public class MainMenuSectionInstaller : MonoInstaller
    {
        [SerializeField] private MainMenuSectionView _mainMenuSectionView;

        public override void InstallBindings()
        {
            Container.Bind<MainMenuSectionController>().AsSingle().NonLazy();
            Container.Bind<MainMenuSectionView>().FromInstance(_mainMenuSectionView).AsSingle().NonLazy();
        }
    }
}
