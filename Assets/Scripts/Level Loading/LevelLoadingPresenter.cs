using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameLogic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace LevelLoading
{
    public class LevelLoadingPresenter
    {
        private LevelLoadingConfig _levelLoadingConfig;
        private LevelLoadingView _levelLoadingView;

        public LevelLoadingPresenter(LevelLoadingView levelLoadingView, LevelLoadingConfig levelLoadingConfig)
        {
            _levelLoadingView = levelLoadingView;
            _levelLoadingConfig = levelLoadingConfig;
        }

        public async UniTask LoadLevelAsync(LevelConfig levelConfig)
        {
            _levelLoadingView.SetActive(true);

            await ShowLoadingLevelThumbnailAsync(levelConfig.ThumbnailReference);
            _levelLoadingView.SetLoadingBarActive(true);

            var sceneLoading = Addressables.LoadSceneAsync(levelConfig.SceneReference);
            while (!sceneLoading.IsDone)
            {
                _levelLoadingView.SetLoadingBarFillAmount(sceneLoading.PercentComplete);
                _levelLoadingView.SetLoadingBarText(_levelLoadingConfig.LoadingText.GetLocalizedString(Mathf.Round(sceneLoading.PercentComplete * 100)));

                await UniTask.Yield();
            }
        }

        public void LoadMainMenu() => SceneManager.LoadScene(0);

        private async UniTask ShowLoadingLevelThumbnailAsync(AssetReference thumbnailReference)
        {
            var levelThumbnail = await Addressables.LoadAssetAsync<Sprite>(thumbnailReference);

            _levelLoadingView.SetBackgroundSprite(levelThumbnail);
            await _levelLoadingView.Background
                .DOFade(1f, 1.5f)
                .AsyncWaitForCompletion();
        }
    }
}
