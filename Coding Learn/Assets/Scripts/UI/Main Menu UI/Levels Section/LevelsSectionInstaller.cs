using UnityEngine;
using Zenject;

namespace Scripts
{
    public class LevelsSectionInstaller : MonoInstaller
    {
        [SerializeField] private LevelsSectionView _levelsSectionView;

        public override void InstallBindings()
        {
            Container.Bind<LevelsSectionController>().AsSingle().NonLazy();
            Container.Bind<LevelsSectionView>().AsSingle().NonLazy();
        }
    }
}
