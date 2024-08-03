using UnityEngine;
using Zenject;

namespace Scripts
{
    public class GameUiInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<TaskDescriptionSectionView>().FromInstance(taskDescriptionSectionView).AsSingle().NonLazy();

            Container.Bind<StorytellingSectionView>().FromInstance(storytellingSectionView).AsSingle().NonLazy();

            Container.Bind<DevEnvironmentSectionView>().FromInstance(padDevEnvironmentView).AsSingle().NonLazy();

            Container.Bind<ChallengesSectionView>().FromInstance(padChallengesScreenView).AsSingle().NonLazy();

            Container.Bind<HandbookSectionView>().FromInstance(padHandbookView).AsSingle().NonLazy();

            Container.Bind<TipsSectionView>().FromInstance(padTipsScreenView).AsSingle().NonLazy();
        }

        [SerializeField] private TaskDescriptionSectionView taskDescriptionSectionView;

        [SerializeField] private StorytellingSectionView storytellingSectionView;

        [SerializeField] private DevEnvironmentSectionView padDevEnvironmentView;

        [SerializeField] private ChallengesSectionView padChallengesScreenView;

        [SerializeField] private HandbookSectionView padHandbookView;

        [SerializeField] private TipsSectionView padTipsScreenView;

    }
}
