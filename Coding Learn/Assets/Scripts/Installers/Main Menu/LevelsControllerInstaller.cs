using UnityEngine;
using Zenject;

namespace Scripts
{
    public class LevelsControllerInstaller : MonoInstaller
    {
        [SerializeField]
        private LevelsSectionView levelsSectionView;

        public override void InstallBindings()
        {
            Container.Bind<LevelsSectionView>().FromInstance(levelsSectionView).AsSingle().NonLazy();
        }
    }
}