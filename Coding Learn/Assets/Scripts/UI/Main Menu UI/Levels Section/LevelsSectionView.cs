using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;

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

        public IEnumerator ChangeVisibility_COR(bool isVisible)
        {
            yield return StartCoroutine(animator.ChangeContentVisibility_COR(isVisible));
        }

        public void CreateLevelButtons(int totalLevelsCount, string[] availableLevelDescriptions, Action<int> buttonPressedAction)
        {
            for (var i = 1; i <= totalLevelsCount; i++)
            {
                var currentLevelNumber = i;

                var levelButton = Instantiate(levelButtonPrefab, levelButtonsContainer.transform);
                levelButton.SetBasicParams(currentLevelNumber);
                if (currentLevelNumber <= availableLevelDescriptions.Length)
                {
                    levelButton.SetActiveButtonParams(availableLevelDescriptions[i - 1], () => buttonPressedAction(currentLevelNumber));
                }
                else
                {
                    levelButton.Deactivate();
                }
            }
        }

        public void SetLevelInfo(string levelTitle)
        {
            levelTitleLabel.text = levelTitle;
        }

        public IEnumerator ShowLoadingScreenContent_COR()
        {
            yield return StartCoroutine(animator.ShowLoadingScreen_COR());
        }

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
                StartCoroutine(UpdateLevelThumbnail_COR());
            }           
            
        }

        public void MakeLevelButtonSelected(int buttonNumber) => levelButtonsContainer.transform.GetChild(buttonNumber - 1).GetComponent<LevelButton>().ClickForce();

        private IEnumerator UpdateLevelThumbnail_COR()
        {
            var previousLevelThumbnail = levelThumbnailContainer.transform.GetChild(0).GetComponent<LevelThumbnail>();
            var newLevelThumbnail = levelThumbnailContainer.transform.GetChild(1).GetComponent<LevelThumbnail>();
            yield return animator.ChangeLevelThumbnail_COR(previousLevelThumbnail, newLevelThumbnail);
            Destroy(previousLevelThumbnail.gameObject);
        }
    }
}
