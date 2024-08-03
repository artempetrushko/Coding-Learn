using UnityEngine;
using Zenject;

namespace Scripts
{
    public class TaskDescriptionSectionInstaller : MonoInstaller
    {
        [SerializeField] private TaskDescriptionSectionView _taskDescriptionSectionView;

        public override void InstallBindings()
        {
            Container.Bind<TaskDescriptionSectionController>().AsSingle().NonLazy();
            Container.Bind<TaskDescriptionSectionView>().FromInstance(_taskDescriptionSectionView).AsSingle().NonLazy();
        }
    }
}
