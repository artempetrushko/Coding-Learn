using Zenject;

namespace Scripts
{
    public class MainMenuControllerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LevelsManager>().AsSingle().NonLazy();
            Container.Bind<SettingsManager>().AsSingle().NonLazy();
            Container.Bind<StatsManager>().AsSingle().NonLazy();
        }
    }
}