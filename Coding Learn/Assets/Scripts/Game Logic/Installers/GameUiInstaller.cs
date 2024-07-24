using UnityEngine;
using Zenject;

namespace Scripts
{
    public class GameUiInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<TaskSectionView>().FromInstance(taskSectionView).AsSingle().NonLazy();
            Container.Bind<TaskDescriptionSectionView>().FromInstance(taskDescriptionSectionView).AsSingle().NonLazy();

            Container.Bind<StorytellingSectionView>().FromInstance(storytellingSectionView).AsSingle().NonLazy();

            Container.Bind<CodingTrainingSectionView>().FromInstance(codingTrainingSectionView).AsSingle().NonLazy();

            Container.Bind<PadDevEnvironmentView>().FromInstance(padDevEnvironmentView).AsSingle().NonLazy();

            Container.Bind<PadChallengesScreenView>().FromInstance(padChallengesScreenView).AsSingle().NonLazy();

            Container.Bind<PadHandbookView>().FromInstance(padHandbookView).AsSingle().NonLazy();

            Container.Bind<PadTipsScreenView>().FromInstance(padTipsScreenView).AsSingle().NonLazy();
        }

        [SerializeField] private TaskSectionView taskSectionView;
        [SerializeField] private TaskDescriptionSectionView taskDescriptionSectionView;

        [SerializeField] private StorytellingSectionView storytellingSectionView;

        [SerializeField] private CodingTrainingSectionView codingTrainingSectionView;

        [SerializeField] private PadDevEnvironmentView padDevEnvironmentView;

        [SerializeField] private PadChallengesScreenView padChallengesScreenView;

        [SerializeField] private PadHandbookView padHandbookView;

        [SerializeField] private PadTipsScreenView padTipsScreenView;

    }
}
