using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Scripts
{
    public class LevelLoadingManager
    {
        private LoadingScreenController _loadingScreenController;

        public LevelLoadingManager(LoadingScreenController loadingScreenController)
        {
            _loadingScreenController = loadingScreenController;
        }

        public async UniTask LoadLevelAsync(LevelData levelData)
        {
            var loadingScreenSprite = await Addressables.LoadAssetAsync<Sprite>(levelData.LoadingScreenReference);
            await _loadingScreenController.ShowAsync(loadingScreenSprite);

            var sceneLoading = Addressables.LoadSceneAsync(levelData.SceneReference);
            while (!sceneLoading.IsDone)
            {
                _loadingScreenController.SetContent(sceneLoading.PercentComplete);
                await UniTask.Yield();
            }
        }

        public void ForceLoadScene(AssetReference sceneReference) => Addressables.LoadSceneAsync(sceneReference);

        private async UniTask ShowLoadingScreenAsync(Sprite loadingScreen)
        {
            _loadingScreenController.gameObject.SetActive(true);
            _loadingScreenController.SetBackgroundSprite(loadingScreen);
            _loadingScreenController.

            loadingScreenView.SetBackground(loadingScreen);
            await _loadingScreenController.ShowAsync();
        }
    }
}
