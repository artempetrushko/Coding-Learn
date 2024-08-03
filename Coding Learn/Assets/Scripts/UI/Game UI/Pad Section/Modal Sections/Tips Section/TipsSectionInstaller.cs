using UnityEngine;
using Zenject;

namespace Scripts
{
    public class TipsSectionInstaller : MonoInstaller
    {
        [SerializeField] private TipsSectionView _tipsScreenView;

        public override void InstallBindings()
        {
            Container.Bind<TipsSectionController>().AsSingle().NonLazy();
            Container.Bind<TipsSectionView>().FromInstance(_tipsScreenView).AsSingle().NonLazy();
        }
    }
}
