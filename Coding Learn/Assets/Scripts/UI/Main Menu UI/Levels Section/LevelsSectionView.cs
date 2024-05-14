using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace Scripts
{
    public class LevelsSectionView : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text levelTitleLabel;
        [SerializeField] 
        private Image loadingBar;
        [SerializeField] 
        private TMP_Text loadingBarText;
        [Space, SerializeField]
        private LevelButton levelButtonPrefab;
        [SerializeField]
        private GameObject levelButtonsContainer;
        [SerializeField]
        private LevelThumbnail levelThumbnailPrefab;
        [SerializeField]
        private GameObject levelThumbnailContainer;
        [Space, SerializeField]
        private LevelsSectionAnimator animator;

        public async UniTask ChangeVisibilityAsync(bool isVisible) => await animator.ChangeContentVisibilityAsync(isVisible);

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

        public async UniTask ShowLoadingScreenContentAsync() => await animator.ShowLoadingScreenAsync();

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
            var levelThumbnails = levelThumbnailContainer.GetComponentsInChildren<LevelThumbnail>();
            await animator.ChangeLevelThumbnailAsync(levelThumbnails[0], levelThumbnails[1]);
            Destroy(levelThumbnails[0].gameObject);
        }
    }
}
