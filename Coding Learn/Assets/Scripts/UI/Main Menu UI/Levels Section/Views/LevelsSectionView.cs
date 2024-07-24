using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace Scripts
{

    public class LevelsSectionView : MonoBehaviour
    {
        [SerializeField] private TMP_Text levelTitleLabel;
        [SerializeField] private Image loadingBar;
        [SerializeField] private TMP_Text loadingBarText;
        [Space, SerializeField]        private LevelButton levelButtonPrefab;
        [SerializeField] private GameObject levelButtonsContainer;
        [SerializeField] private LevelThumbnailView levelThumbnailPrefab;
        [SerializeField] private GameObject levelThumbnailContainer;

        public async UniTask ChangeVisibilityAsync(bool isVisible) => await ChangeContentVisibilityAsync(isVisible);

        public void SetLevelInfo(string levelTitle)
        {
            levelTitleLabel.text = levelTitle;
        }

        public void CreateLevelButtons(int totalLevelsCount)
        {
            for (var i = 1; i <= totalLevelsCount; i++)
            {
                var levelButton = Instantiate(levelButtonPrefab, levelButtonsContainer.transform);
                levelButton.SetInteractivity(false);
                levelButton.SetBasicParams(i);
            }
        }

        public void SetActiveLevelButtonsParams(LevelData[] levelDatas, Action<LevelData> buttonPressedAction)
        {
            for (var i = 1; i <= levelDatas.Length; i++)
            {
                var levelNumber = i;
                var levelButton = levelButtonsContainer.transform.GetChild(levelNumber - 1).GetComponent<LevelButton>();
                var levelData = levelDatas[levelNumber - 1];
                levelButton.SetInteractivity(true);
                levelButton.SetActiveButtonParams(levelData.Description.GetLocalizedString(), () => buttonPressedAction(levelData));
            }
        }

        public async UniTask ShowLoadingScreenContentAsync() => await ShowLoadingScreenAsync();

        public void SetLoadingBarInfo(float loadingOperationProgress)
        {
            loadingBar.fillAmount = loadingOperationProgress;
            loadingBarText.text = loadingBarText.GetComponent<LocalizeStringEvent>().StringReference.GetLocalizedString(Mathf.Round(loadingOperationProgress * 100));
        }

        public void SetLevelThumbnail(Sprite newThumbnail)
        {
            var newLevelThumbnail = Instantiate(levelThumbnailPrefab, levelThumbnailContainer.transform);
            newLevelThumbnail.SetContent(newThumbnail);
            if (levelThumbnailContainer.transform.childCount > 1)
            {
                _ = UpdateLevelThumbnailAsync();
            }            
        }

        public void MakeLevelButtonSelected(int buttonNumber) => levelButtonsContainer.transform.GetChild(buttonNumber - 1).GetComponent<LevelButton>().ClickForce();

        private async UniTask UpdateLevelThumbnailAsync()
        {
            var levelThumbnails = levelThumbnailContainer.GetComponentsInChildren<LevelThumbnailView>();
            await ChangeLevelThumbnailAsync(levelThumbnails[0], levelThumbnails[1]);
            Destroy(levelThumbnails[0].gameObject);
        }




        [SerializeField]
        private GameObject header;
        [SerializeField]
        private Image blackScreen;
        [SerializeField]
        private GameObject loadingBarContainer;

        public async UniTask ChangeContentVisibilityAsync(bool isVisible)
        {
            var thumbnailContainerEndAlpha = isVisible ? 1f : 0f;
            var visibilityChangeDuration = 1f;

            ChangeMainContentVisibility(isVisible, visibilityChangeDuration);
            //levelThumbnailContainer.DOFade(thumbnailContainerEndAlpha, visibilityChangeDuration);
            await UniTask.Delay(TimeSpan.FromSeconds(visibilityChangeDuration));
        }

        public async UniTask ChangeLevelThumbnailAsync(LevelThumbnailView previousThumbnail, LevelThumbnailView newThumbnail)
        {
            var thumbnailChangeDuration = 1f;
            previousThumbnail.Image.DOFillAmount(0f, thumbnailChangeDuration);
            newThumbnail.Image.DOFillAmount(1f, thumbnailChangeDuration);
            await UniTask.Delay(TimeSpan.FromSeconds(thumbnailChangeDuration));
        }

        public async UniTask ShowLoadingScreenAsync()
        {
            blackScreen.gameObject.SetActive(true);
            await ChangeBlackScreenVisibilityAsync(true);
            ChangeMainContentVisibility(false, 0f);
            loadingBarContainer.transform.DOLocalMoveY(loadingBarContainer.transform.localPosition.y + loadingBarContainer.GetComponent<RectTransform>().rect.height, 0f);
            await ChangeBlackScreenVisibilityAsync(false);
            blackScreen.gameObject.SetActive(false);
        }

        private async UniTask ChangeBlackScreenVisibilityAsync(bool isVisible)
        {
            var blackScreenEndOpacity = isVisible ? 1f : 0f;
            await blackScreen
                .DOFade(blackScreenEndOpacity, 1.5f)
                .AsyncWaitForCompletion();
        }

        private void ChangeMainContentVisibility(bool isVisible, float movementDuration)
        {
            var movementOffsetYSign = isVisible ? 1 : -1;
            MoveContentY(header, -movementOffsetYSign, movementDuration);
            MoveContentY(levelButtonsContainer, movementOffsetYSign, movementDuration);
        }

        private void MoveContentY(GameObject content, int movementSign, float movementDuration)
            => content.transform.DOLocalMoveY(content.transform.localPosition.y + (content.GetComponent<RectTransform>().rect.height * movementSign), movementDuration);
    }
}
