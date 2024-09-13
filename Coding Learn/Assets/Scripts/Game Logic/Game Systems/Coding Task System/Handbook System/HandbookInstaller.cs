using GameLogic;
using UnityEngine;
using Zenject;

namespace UI.Game
{
    public class HandbookInstaller : MonoInstaller
    {
        [SerializeField] private HandbookView _handbookView;

        public override void InstallBindings()
        {
            Container.Bind<HandbookPresenter>().AsSingle().NonLazy();
            Container.Bind<HandbookView>().FromInstance(_handbookView).AsSingle().NonLazy();
        }
    }
}
