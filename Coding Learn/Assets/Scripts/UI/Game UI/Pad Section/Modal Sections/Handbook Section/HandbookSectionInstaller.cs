using UnityEngine;
using Zenject;

namespace Scripts
{
    public class HandbookSectionInstaller : MonoInstaller
    {
        [SerializeField] private HandbookSectionView _handbookView;

        public override void InstallBindings()
        {
            Container.Bind<HandbookSectionController>().AsSingle().NonLazy();
            Container.Bind<HandbookSectionView>().FromInstance(_handbookView).AsSingle().NonLazy();
        }
    }
}
