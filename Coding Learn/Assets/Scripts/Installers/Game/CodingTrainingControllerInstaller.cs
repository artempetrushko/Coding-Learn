using UnityEngine;
using Zenject;

namespace Scripts
{
    public class CodingTrainingControllerInstaller : MonoInstaller
    {
        [SerializeField]
        private CodingTrainingSectionView codingTrainingSectionView;

        public override void InstallBindings()
        {
            Container.Bind<CodingTrainingSectionView>().FromInstance(codingTrainingSectionView).AsSingle().NonLazy();
            Container.Bind<CodingTrainingController>().ToSelf().AsSingle().NonLazy();
        }
    }
}