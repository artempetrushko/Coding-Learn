using GameLogic;
using UnityEngine;
using Zenject;

namespace GameLogic
{
    public class CodingTaskInstaller : MonoInstaller
    {
        [SerializeField] private CodingTaskView _view;

        public override void InstallBindings()
        {
            Container.Bind<CodingTaskPresenter>().AsSingle().NonLazy();
            Container.Bind<CodingTaskView>().FromInstance(_view).AsSingle().NonLazy();
        }
    }
}
