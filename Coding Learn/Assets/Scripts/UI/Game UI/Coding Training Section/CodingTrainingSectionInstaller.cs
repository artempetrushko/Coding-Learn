using UnityEngine;
using Zenject;

namespace Scripts
{
    public class CodingTrainingSectionInstaller : MonoInstaller
    {
        [SerializeField] private CodingTrainingSectionView _codingTrainingSectionView;

        public override void InstallBindings()
        {
            Container.Bind<CodingTrainingSectionController>().AsSingle().NonLazy();
            Container.Bind<CodingTrainingSectionView>().FromInstance(_codingTrainingSectionView).AsSingle().NonLazy();
        }
    }
}
