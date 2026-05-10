using UnityEngine;
using Zenject;

namespace GameLogic
{
    public class ChallengesInstaller : MonoInstaller
    {
        [SerializeField] private ChallengesView _challengesScreenView;

        public override void InstallBindings()
        {
            Container.Bind<ChallengesPresenter>().AsSingle().NonLazy();
            Container.Bind<ChallengesView>().FromInstance(_challengesScreenView).AsSingle().NonLazy();
        }
    }
}
