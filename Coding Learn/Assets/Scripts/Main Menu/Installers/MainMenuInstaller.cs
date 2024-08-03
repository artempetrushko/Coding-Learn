using Zenject;

namespace Scripts
{
    public class MainMenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LevelsManager>().AsSingle().NonLazy();
            Container.Bind<SettingsManager>().AsSingle().NonLazy();
            Container.Bind<StatsManager>().AsSingle().NonLazy();
            Container.Bind<SoundsManager>().AsSingle().NonLazy();
        }
    }
}