using Core.Features.Game.Storytelling;
using GameLogic;
using Scripts.Assets.Scripts.UI.Game.Training;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;
using Zenject;

namespace Scripts.Bootstrap.Game
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameConfig _gameConfig;
        [Space]
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private TrainingPageComponent _trainingView;
        [SerializeField] private StorytellingView _storytellingView;
        [SerializeField] private PlayableDirector _playableDirector;
        [SerializeField] private CodingTaskComponent _codingTaskView;
        [SerializeField] private TaskTipsСomponent _taskTipsView;
        [SerializeField] private HandbookComponent _handbookView;
        [SerializeField] private DevEnvironmentComponent _devEnvironmentview;
        [SerializeField] private ChallengesView _challengesScreenView;
        [SerializeField] private ExitMenuComponent _exitMenuView;

        public override void InstallBindings()
        {
            Container.Bind<GameManager>().AsSingle().NonLazy();
            Container.Bind<GameConfig>().FromScriptableObject(_gameConfig).AsSingle().NonLazy();

            Container.Bind<TrainingPresenter>().AsSingle().NonLazy();
            Container.Bind<TrainingPageComponent>().FromInstance(_trainingView).AsSingle().NonLazy();

            Container.Bind<StorytellingService>().AsSingle().NonLazy();
            Container.Bind<StorytellingView>().FromInstance(_storytellingView).AsSingle().NonLazy();
            Container.Bind<PlayableDirector>().FromInstance(_playableDirector).AsSingle().NonLazy();

            Container.Bind<CodingTaskPresenter>().AsSingle().NonLazy();
            Container.Bind<CodingTaskComponent>().FromInstance(_codingTaskView).AsSingle().NonLazy();

            Container.Bind<TaskTipsPresenter>().AsSingle().NonLazy();
            Container.Bind<TaskTipsСomponent>().FromInstance(_taskTipsView).AsSingle().NonLazy();

            Container.Bind<HandbookPresenter>().AsSingle().NonLazy();
            Container.Bind<HandbookComponent>().FromInstance(_handbookView).AsSingle().NonLazy();

            Container.Bind<DevEnvironmentPresenter>().AsSingle().NonLazy();
            Container.Bind<DevEnvironmentComponent>().FromInstance(_devEnvironmentview).AsSingle().NonLazy();

            Container.Bind<ChallengesPresenter>().AsSingle().NonLazy();
            Container.Bind<ChallengesView>().FromInstance(_challengesScreenView).AsSingle().NonLazy();

            Container.Bind<ExitMenuComponent>().FromInstance(_exitMenuView).AsSingle().NonLazy();

            Container.Bind<AudioMixer>().FromInstance(_audioMixer).AsSingle().NonLazy();
        }
    }
}
