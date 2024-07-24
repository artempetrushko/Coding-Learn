using UnityEngine;
using UnityEngine.Playables;
using Zenject;

namespace Scripts
{
    public class GameSystemsInstaller : MonoInstaller
    {
        [SerializeField]
        private PlayableDirector playableDirector;

        public override void InstallBindings()
        {
            Container.Bind<PlayableDirector>().FromInstance(playableDirector).AsSingle().NonLazy();

            Container.Bind<StorytellingManager>().AsSingle().NonLazy();
            Container.Bind<QuestManager>().AsSingle().NonLazy();
            Container.Bind<GameTaskManager>().AsSingle().NonLazy();
            Container.Bind<HandbookManager>().AsSingle().NonLazy();
            Container.Bind<DevEnvironmentManager>().AsSingle().NonLazy();
            Container.Bind<TrainingManager>().AsSingle().NonLazy();
            Container.Bind<ChallengeManager>().AsSingle().NonLazy();
            Container.Bind<TaskTipManager>().AsSingle().NonLazy();
        }
    }
}
