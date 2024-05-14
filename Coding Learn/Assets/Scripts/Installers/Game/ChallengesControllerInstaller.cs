using UnityEngine;
using Zenject;

namespace Scripts
{
    public class ChallengesControllerInstaller : MonoInstaller
    {
        [SerializeField]
        private PadChallengesScreenView padChallengesScreenView;

        public override void InstallBindings()
        {
            Container.Bind<PadChallengesScreenView>().FromInstance(padChallengesScreenView).AsSingle().NonLazy();
            Container.Bind<ChallengesController>().ToSelf().AsSingle().NonLazy();
        }
    }
}