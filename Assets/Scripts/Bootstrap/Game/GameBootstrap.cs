using GameLogic;
using UnityEngine;
using Zenject;

namespace Scripts.Bootstrap.Game
{
    public class GameBootstrap : MonoBehaviour
    {
        private GameManager _gameManager;

        [Inject]
        public void Construct(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

		private void Start()
		{
            _gameManager.StartGame();
		}
	}
}
