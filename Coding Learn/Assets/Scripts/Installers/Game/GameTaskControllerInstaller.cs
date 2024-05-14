using UnityEngine;
using Zenject;

namespace Scripts
{
    public class GameTaskControllerInstaller : MonoInstaller
    {
        [SerializeField]
        private TaskSectionView taskSectionView;
        [SerializeField]
        private TaskDescriptionSectionView taskDescriptionSectionView;

        public override void InstallBindings()
        {
            Container.Bind<TaskSectionView>().FromInstance(taskSectionView).AsSingle().NonLazy();
            Container.Bind<TaskDescriptionSectionView>().FromInstance(taskDescriptionSectionView).AsSingle().NonLazy();
            Container.Bind<GameTaskController>().ToSelf().AsSingle().NonLazy();
        }
    }
}