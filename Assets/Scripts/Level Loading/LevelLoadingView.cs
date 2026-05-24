using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameLogic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LevelLoading
{
    public class LevelLoadingView : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [Space]
        [SerializeField] private GameObject _loadingBar;
        [SerializeField] private Image _loadingBarInnerArea;
        [SerializeField] private TMP_Text _loadingBarText;
        [SerializeField] private LocalizedString _loadingText;

        public async UniTask LoadLevelAsync(LevelConfig levelConfig)
        {
            gameObject.SetActive(true);

            await ShowLoadingLevelThumbnailAsync(levelConfig.ThumbnailReference);
            _loadingBar.SetActive(true);

            var sceneLoading = Addressables.LoadSceneAsync(levelConfig.SceneReference);
            while (!sceneLoading.IsDone)
            {
                _loadingBarInnerArea.fillAmount = sceneLoading.PercentComplete;
                _loadingBarText.text = _loadingText.GetLocalizedString(Mathf.Round(sceneLoading.PercentComplete * 100));

                await UniTask.Yield();
            }
        }

        public void LoadMainMenu() => SceneManager.LoadScene(0);

        private async UniTask ShowLoadingLevelThumbnailAsync(AssetReference thumbnailReference)
        {
            var levelThumbnail = await Addressables.LoadAssetAsync<Sprite>(thumbnailReference);

            _background.sprite = levelThumbnail;
            await _background
                .DOFade(1f, 1.5f)
                .AsyncWaitForCompletion();
        }
    }
}
