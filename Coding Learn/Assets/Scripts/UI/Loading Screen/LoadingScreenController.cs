using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Scripts
{
    public class LoadingScreenController
    {
        private LoadingScreenView _loadingScreenView;

        public LoadingScreenController(LoadingScreenView loadingScreenView)
        {
            _loadingScreenView = loadingScreenView;
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
            _loadingScreenView.SetLoadingBarText(/*_loadingBarText.GetComponent<LocalizeStringEvent>().StringReference.GetLocalizedString(Mathf.Round(loadingProgress * 100));*/);
        }
    }
}
