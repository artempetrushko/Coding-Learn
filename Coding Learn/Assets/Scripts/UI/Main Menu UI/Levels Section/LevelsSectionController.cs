using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Scripts
{
    public class LevelsSectionController 
    {
        private const float THUMBNAIL_CHANGING_DURATION = 1f;

        private LevelsSectionView _levelsSectionView;
        private LevelButton levelButtonPrefab;
        private LevelThumbnailView levelThumbnailPrefab;
        private DiContainer _diContainer;

        public LevelsSectionController(DiContainer diContainer, LevelsSectionView levelsSectionView) 
        { 
            _levelsSectionView = levelsSectionView;
            _diContainer = diContainer;
        }

        public async UniTask SetVisibilityAsync(bool isVisible)
        {
            var thumbnailContainerEndAlpha = isVisible ? 1f : 0f;
            var visibilityChangeDuration = 1f;

            SetMainContentVisibilityAsync(isVisible, visibilityChangeDuration);
            //levelThumbnailContainer.DOFade(thumbnailContainerEndAlpha, visibilityChangeDuration);
            await UniTask.WaitForSeconds(visibilityChangeDuration);
        }

        public void SetLevelInfo(string levelTitle)
        {
            _levelsSectionView.SetLevelTitleText(levelTitle);
        }

        public void CreateLevelButtons(int totalLevelsCount)
        {
            for (var i = 1; i <= totalLevelsCount; i++)
            {
                var levelButton = _diContainer.InstantiatePrefab(levelButtonPrefab, _levelsSectionView.LevelButtonsContainer.transform).GetComponent<LevelButton>();
                levelButton.SetInteractivity(false);
                levelButton.SetBasicParams(i);
            }
        }

        public void SetActiveLevelButtonsParams(LevelData[] levelDatas, Action<LevelData> buttonPressedAction)
        {
            for (var i = 1; i <= levelDatas.Length; i++)
            {
                var levelNumber = i;
                var levelButton = _levelsSectionView.LevelButtonsContainer.transform.GetChild(levelNumber - 1).GetComponent<LevelButton>();
                var levelData = levelDatas[levelNumber - 1];
                levelButton.SetInteractivity(true);
                levelButton.SetActiveButtonParams(levelData.Description.GetLocalizedString(), () => buttonPressedAction(levelData));
            }
        }

        public void SetLoadingBarInfo(float loadingOperationProgress)
        {
            _levelsSectionView.SetLoadingBarFillAmount(loadingOperationProgress);
            _levelsSectionView.SetLoadingBarText(/*GetLocalizedString(Mathf.Round(loadingOperationProgress * 100))*/);
        }

        public void SetLevelThumbnail(Sprite newThumbnail)
        {
            var newLevelThumbnail = _diContainer.InstantiatePrefab(levelThumbnailPrefab, levelThumbnailContainer.transform).GetComponent<LevelThumbnailView>();
            newLevelThumbnail.SetThumbnail(newThumbnail);
            if (levelThumbnailContainer.transform.childCount > 1)
            {
                UpdateLevelThumbnailAsync().Forget();
            }
        }

        public void MakeLevelButtonSelected(int buttonNumber) => levelButtonsContainer.transform.GetChild(buttonNumber - 1).GetComponent<LevelButton>().ClickForce();

        private async UniTask UpdateLevelThumbnailAsync()
        {
            var levelThumbnails = levelThumbnailContainer.GetComponentsInChildren<LevelThumbnailView>();
            await ChangeLevelThumbnailAsync(levelThumbnails[0], levelThumbnails[1]);
            Object.Destroy(levelThumbnails[0].gameObject);
        }

        public async UniTask ChangeLevelThumbnailAsync(LevelThumbnailView previousThumbnail, LevelThumbnailView newThumbnail)
        {
            previousThumbnail.SetThumbnailFillAmountAsync(0f, THUMBNAIL_CHANGING_DURATION).Forget();
            newThumbnail.SetThumbnailFillAmountAsync(1f, THUMBNAIL_CHANGING_DURATION).Forget();

            await UniTask.WaitForSeconds(THUMBNAIL_CHANGING_DURATION);
        }

        public async UniTask ShowLoadingScreenAsync()
        {
            _levelsSectionView.SetBlackScreenActive(true);
            await _levelsSectionView.SetBlackScreenVisibilityAsync(true);

            SetMainContentVisibilityAsync(false, 0f).Forget();
            _levelsSectionView.SetLoadingBarContainerVisibilityAsync(true, 0f).Forget();

            await _levelsSectionView.SetBlackScreenVisibilityAsync(false);
            _levelsSectionView.SetBlackScreenActive(false);
        }

        private async UniTask SetMainContentVisibilityAsync(bool isVisible, float duration)
        {
            _levelsSectionView.SetHeaderVisibilityAsync(isVisible, duration).Forget();
            _levelsSectionView.SetLevelButtonsContainerVisibilityAsync(isVisible, duration).Forget();

            await UniTask.WaitForSeconds(duration);
        }
    }
}
