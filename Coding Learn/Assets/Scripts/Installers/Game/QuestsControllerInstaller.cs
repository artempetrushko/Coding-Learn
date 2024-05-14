using UnityEngine;
using Zenject;

namespace Scripts
{
    public class QuestsControllerInstaller : MonoInstaller
    {
        [SerializeField]
        private StorytellingController storytellingController;

        public override void InstallBindings()
        {
            Container.Bind<StorytellingController>().FromInstance(storytellingController).AsSingle().NonLazy();
            Container.Bind<QuestsController>().ToSelf().AsSingle().NonLazy();
        }
    }
}