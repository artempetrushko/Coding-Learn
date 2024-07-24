using UnityEngine;
using Zenject;

namespace Scripts
{
    public class StatsControllerInstaller : MonoInstaller
    {
        [SerializeField]
        private StatsSectionView statsSectionView;

        public override void InstallBindings()
        {
            Container.Bind<StatsSectionView>().FromInstance(statsSectionView).AsSingle().NonLazy();
        }
    }
}