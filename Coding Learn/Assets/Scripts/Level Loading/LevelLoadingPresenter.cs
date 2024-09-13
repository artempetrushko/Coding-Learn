using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameLogic;
using UI.LoadingScreen;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace LevelLoading
{
	public class LevelLoadingPresenter
    {
        private LevelLoadingView _loadingScreenView;

        public LevelLoadingPresenter(LevelLoadingView loadingScreenView)
        {
            _loadingScreenView = loadingScreenView;
        }

        public async UniTask LoadLevelAsync(LevelConfig levelData)
        {
            var loadingScreenSprite = await Addressables.LoadAssetAsync<Sprite>(levelData.LoadingScreenReference);
            await ShowAsync(loadingScreenSprite);

            var sceneLoading = Addressables.LoadSceneAsync(levelData.SceneReference);
            while (!sceneLoading.IsDone)
            {
                SetContent(sceneLoading.PercentComplete);
                await UniTask.Yield();
            }
        }

        public void LoadMainMenu() => SceneManager.LoadScene(0);

        private async UniTask ShowLoadingScreenAsync(Sprite loadingScreen)
        {
           /* _loadingScreenController.gameObject.SetActive(true);
            _loadingScreenController.SetBackgroundSprite(loadingScreen);
            _loadingScreenController.

            loadingScreenView.SetBackground(loadingScreen);
            await _loadingScreenController.ShowAsync();*/
        }




        

        

        public async UniTask ShowAsync(Sprite loadingScreenSprite)
        {
            _loadingScreenView.SetBackgroundSprite(loadingScreenSprite);

            await _loadingScreenView.Background
                .DOFade(1f, 1.5f)
                .AsyncWaitForCompletion();
            _loadingScreenView.SetLoadingBarActive(true);
        }

        public void SetContent(float loadingProgress)
        {
            _loadingScreenView.SetLoadingBarFillAmount(loadingProgress);
            //_loadingScreenView.SetLoadingBarText(/*_loadingBarText.GetComponent<LocalizeStringEvent>().StringReference.GetLocalizedString(Mathf.Round(loadingProgress * 100));*/);
        }
    }
}
