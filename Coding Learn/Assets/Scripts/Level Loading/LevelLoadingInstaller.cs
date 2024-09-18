using UnityEngine;
using Zenject;

namespace LevelLoading
{
    public class LevelLoadingInstaller : MonoInstaller
    {
		[SerializeField] private LevelLoadingView _view;
		[SerializeField] private LevelLoadingConfig _config;

		public override void InstallBindings()
		{
			Container.Bind<LevelLoadingPresenter>().AsSingle().NonLazy();
            Container.Bind<LevelLoadingView>().FromInstance(_view).AsSingle().NonLazy();
			Container.Bind<LevelLoadingConfig>().FromScriptableObject(_config).AsSingle().NonLazy();
        }
	}
}
