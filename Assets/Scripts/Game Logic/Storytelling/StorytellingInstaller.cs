using UnityEngine;
using UnityEngine.Playables;
using Zenject;

namespace GameLogic
{
    public class StorytellingInstaller : MonoInstaller
    {
        [SerializeField] private StorytellingView _view;
        [SerializeField] private PlayableDirector _playableDirector;

        public override void InstallBindings()
        {
            Container.Bind<StorytellingPresenter>().AsSingle().NonLazy();
            Container.Bind<StorytellingView>().FromInstance(_view).AsSingle().NonLazy();
            Container.Bind<PlayableDirector>().FromInstance(_playableDirector).AsSingle().NonLazy();
        }
    }
}
