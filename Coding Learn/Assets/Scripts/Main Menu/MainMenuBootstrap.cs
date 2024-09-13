using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MainMenu
{
    public class MainMenuBootstrap : MonoBehaviour
    {
        private MainMenuPresenter _mainMenuPresenter;

        [Inject]
        public void Construct(MainMenuPresenter mainMenuPresenter)
        {
            _mainMenuPresenter = mainMenuPresenter;
        }

        private void Start()
        {
            _mainMenuPresenter.StartAsync().Forget();
        }
    }
}