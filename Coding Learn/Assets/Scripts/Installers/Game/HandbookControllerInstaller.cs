using UnityEngine;
using Zenject;

namespace Scripts
{
    public class HandbookControllerInstaller : MonoInstaller
    {
        [SerializeField]
        private PadHandbookView padHandbookView;

        public override void InstallBindings()
        {
            Container.Bind<PadHandbookView>().FromInstance(padHandbookView).AsSingle().NonLazy();
            Container.Bind<HandbookController>().ToSelf().AsSingle().NonLazy();
        }
    }
}