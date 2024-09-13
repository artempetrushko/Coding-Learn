using GameLogic;
using UnityEngine;
using Zenject;

namespace UI.Game
{
    public class TrainingInstaller : MonoInstaller
    {
        [SerializeField] private TrainingView _view;

        public override void InstallBindings()
        {
            Container.Bind<TrainingPresenter>().AsSingle().NonLazy();
            Container.Bind<TrainingView>().FromInstance(_view).AsSingle().NonLazy();
        }
    }
}
