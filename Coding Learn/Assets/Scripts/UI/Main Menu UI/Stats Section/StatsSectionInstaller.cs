using UnityEngine;
using Zenject;

namespace Scripts
{
    public class StatsSectionInstaller : MonoInstaller
    {
        [SerializeField] private StatsSectionView _statsSectionView;
        [SerializeField] private LevelStatsCardView _levelStatsCardPrefab;
        [SerializeField] private TaskStatsView _taskStatsPrefab;

        public override void InstallBindings()
        {
            Container.Bind<StatsSectionController>().AsSingle().NonLazy();
            Container.Bind<StatsSectionView>().FromInstance(_statsSectionView).AsSingle().NonLazy();
            Container.Bind<LevelStatsCardView>().FromComponentInNewPrefab(_levelStatsCardPrefab).AsSingle().NonLazy();
            Container.Bind<TaskStatsView>().FromComponentInNewPrefab(_taskStatsPrefab).AsSingle().NonLazy();
        }
    }
}
