using UnityEngine;
using Zenject;

namespace Scripts
{
    public class StorytellingSectionInstaller : MonoInstaller
    {
        [SerializeField] private StorytellingSectionView _storytellingSectionView;

        public override void InstallBindings()
        {
            Container.Bind<StorytellingSectionController>().AsSingle().NonLazy();
            Container.Bind<StorytellingSectionView>().FromInstance(_storytellingSectionView).AsSingle().NonLazy();
        }
    }
}
