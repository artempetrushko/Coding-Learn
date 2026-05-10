using GameLogic;
using UnityEngine;
using Zenject;

namespace GameLogic
{
    public class TaskTipsInstaller : MonoInstaller
    {
        [SerializeField] private TaskTipsView _taskTipsView;

        public override void InstallBindings()
        {
            Container.Bind<TaskTipsPresenter>().AsSingle().NonLazy();
            Container.Bind<TaskTipsView>().FromInstance(_taskTipsView).AsSingle().NonLazy();
        }
    }
}
