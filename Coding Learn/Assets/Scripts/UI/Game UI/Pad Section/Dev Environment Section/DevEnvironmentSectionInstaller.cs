using UnityEngine;
using Zenject;

namespace Scripts
{
    public class DevEnvironmentSectionInstaller : MonoInstaller
    {
        [SerializeField] private DevEnvironmentSectionView _devEnvironmentSectionView;

        public override void InstallBindings()
        {
            Container.Bind<DevEnvironmentSectionController>().AsSingle().NonLazy();
            Container.Bind<DevEnvironmentSectionView>().FromInstance(_devEnvironmentSectionView).AsSingle().NonLazy();
        }
    }
}
