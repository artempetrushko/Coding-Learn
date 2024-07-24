using UnityEngine;
using Zenject;

namespace Scripts
{
    public class GameStartupInstaller : MonoInstaller
    {
        [SerializeField]
        private GameData gameData;

        public override void InstallBindings()
        {
            Container.Bind<GameData>().FromScriptableObject(gameData).AsSingle().NonLazy();
            Container.Bind<SaveManager>().AsSingle().NonLazy();
        }
    }
}