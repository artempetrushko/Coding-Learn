using Zenject;

namespace Scripts
{
    public class MainMenuControllerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LevelsController>().ToSelf().AsSingle().NonLazy();
            Container.Bind<SettingsController>().ToSelf().AsSingle().NonLazy();
            Container.Bind<StatsController>().ToSelf().AsSingle().NonLazy();
        }
    }
}