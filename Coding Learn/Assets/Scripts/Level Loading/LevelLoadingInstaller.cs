using UI.LoadingScreen;
using UnityEngine;
using Zenject;

namespace LevelLoading
{
	public class LevelLoadingInstaller : MonoInstaller
    {
		[SerializeField] private LevelLoadingView _view;

		public override void InstallBindings()
		{
			Container.Bind<LevelLoadingPresenter>().AsSingle().NonLazy();
            Container.Bind<LevelLoadingView>().FromInstance(_view).AsSingle().NonLazy();
        }
	}
}
