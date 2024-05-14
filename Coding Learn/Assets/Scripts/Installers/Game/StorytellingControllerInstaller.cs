using UnityEngine;
using UnityEngine.Playables;
using Zenject;

namespace Scripts
{
    public class StorytellingControllerInstaller : MonoInstaller
    {
        [SerializeField]
        private StorytellingSectionView storytellingSectionView;
        [SerializeField]
        private PlayableDirector playableDirector;

        public override void InstallBindings()
        {
            Container.Bind<PlayableDirector>().FromInstance(playableDirector).AsSingle().NonLazy();
            Container.Bind<StorytellingSectionView>().FromInstance(storytellingSectionView).AsSingle().NonLazy();
            Container.Bind<StorytellingController>().ToSelf().AsSingle().NonLazy();
        }
    }
}