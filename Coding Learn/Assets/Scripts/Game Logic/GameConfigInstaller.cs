using UnityEngine;
using Zenject;

namespace GameLogic
{
    public class GameConfigInstaller : MonoInstaller
    {
        [SerializeField] private GameConfig _gameConfig;

        public override void InstallBindings()
        {
            Container.Bind<GameConfig>().FromScriptableObject(_gameConfig).AsSingle().NonLazy();
        }
    }
}
