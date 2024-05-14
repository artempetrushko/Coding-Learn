using Zenject;

namespace Scripts
{
    public class PadControllerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PadController>().ToSelf().AsSingle().NonLazy();
        }
    }
}