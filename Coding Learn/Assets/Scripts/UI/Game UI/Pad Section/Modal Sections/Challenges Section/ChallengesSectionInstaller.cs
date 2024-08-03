using UnityEngine;
using Zenject;

namespace Scripts
{
    public class ChallengesSectionInstaller : MonoInstaller
    {
        [SerializeField] private ChallengesSectionView _challengesScreenView;
        [SerializeField] private PadChallengeView _challengeViewPrefab;

        public override void InstallBindings()
        {
            Container.Bind<ChallengesSectionController>().AsSingle().NonLazy();
            Container.Bind<ChallengesSectionView>().FromInstance(_challengesScreenView).AsSingle().NonLazy();
            Container.Bind<PadChallengeView>().FromComponentInNewPrefab(_challengeViewPrefab).AsSingle().NonLazy();
        }
    }
}
