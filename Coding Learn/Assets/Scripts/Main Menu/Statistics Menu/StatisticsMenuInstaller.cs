using MainMenu;
using UnityEngine;
using Zenject;

namespace UI.MainMenu
{
    public class StatisticsMenuInstaller : MonoInstaller
    {
        [SerializeField] private StatisticsMenuView _view;

        public override void InstallBindings()
        {
            Container.Bind<StatisticsMenuPresenter>().AsSingle().NonLazy();
            Container.Bind<StatisticsMenuView>().FromInstance(_view).AsSingle().NonLazy();
        }
    }
}
