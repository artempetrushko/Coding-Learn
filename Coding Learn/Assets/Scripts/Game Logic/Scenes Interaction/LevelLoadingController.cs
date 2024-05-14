using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Scripts
{
    public class LevelLoadingController
    {
        private LoadingScreenView loadingScreenView;

        public LevelLoadingController(LoadingScreenView loadingScreenView)
        {
            this.loadingScreenView = loadingScreenView;
        }

        public async UniTask LoadLevelAsync(LevelData levelData)
        {
            //await ShowLoadingScreenAsync(levelData.LoadingScreen);

            var sceneLoading = Addressables.LoadSceneAsync(levelData.SceneReference);
            while (!sceneLoading.IsDone)
            {
                loadingScreenView.SetLoadingBarState(sceneLoading.PercentComplete);
                await UniTask.Yield();
            }
        }

        public void ForceLoadScene(AssetReference sceneReference) => Addressables.LoadSceneAsync(sceneReference);

        private async UniTask ShowLoadingScreenAsync(Sprite loadingScreen)
        {
            loadingScreenView.gameObject.SetActive(true);
            loadingScreenView.SetBackground(loadingScreen);
            await loadingScreenView.ShowAsync();
        }
    }
}
